using AutoMapper;
using System.Transactions;
using SecurePass.Common.Models;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;
using SecurePass.Common.Constants;
using SecurePass.Common.Exceptions;
using SecurePass.Services.Contracts;

namespace SecurePass.Services;

public class AccountService(
    IMapper mapper,
    IJwtService jwtService,
    IUserRepository userRepository,
    ITempUserRepository tempUserRepository,
    ICryptographyService cryptographyService,
    IBackgroundJobService backgroundJobService,
    IRefreshTokenEntryRepository refreshTokenEntryRepository
    ) : IAccountService
{
    private readonly IMapper _mapper = mapper;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITempUserRepository _tempUserRepository = tempUserRepository;
    private readonly ICryptographyService _cryptographyService = cryptographyService;
    private readonly IBackgroundJobService _backgroundJobService = backgroundJobService;
    private readonly IRefreshTokenEntryRepository _refreshTokenEntryRepository = refreshTokenEntryRepository;

    public async Task<List<UserModel>> GetAllUsers(int userId)
    {
        var users = await this._userRepository.GetList(predicate: user => user.Id != userId);
        return _mapper.Map<List<UserModel>>(users);
    }

    public async Task<bool> AccountExists(string userEmail)
    {
        var existingUser = await this.GetUser(userEmail);
        return existingUser != null;
    }

    public async Task<string> GetSalt(SaltRequestModel saltRequest)
    {
        var user = await this.GetUser(saltRequest.UserEmail)
            ?? throw new ApiException(message: "User account with this email doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        return user.RandomSalt;
    }

    public async Task<JwtTokenContainerModel> Signin(SigninRequestModel signinRequest)
    {
        var user = await this.GetUser(signinRequest.UserEmail) 
            ?? throw new ApiException(message: "User account with this email doesn't exist.", errorCode: AppConstants.ErrorCodeEnum.NotFound);

        bool isValidAuthenticationKey = this._cryptographyService.VerifyHash(signinRequest.AuthenticationKey, user.AuthenticationHash);
        if (!isValidAuthenticationKey)
        {
            throw new ApiException(message: "Incorrect email or password provided.", errorCode: AppConstants.ErrorCodeEnum.Unauthorized);
        }

        return this._jwtService.GetJwtTokens(user);
    }

    public async Task Logout(int userId)
    {
        var refreshTokenEntries = await this.GetRefreshTokensByUserId(userId);
        await this._refreshTokenEntryRepository.DeleteRange(refreshTokenEntries);
    }

    public async Task<JwtTokenContainerModel> RefreshTokens(RefreshTokenRequestModel refreshTokenRequest)
    {
        bool isValidRefreshToken = this._jwtService.ValidateRefreshToken(refreshTokenRequest.RefreshToken);
        if (!isValidRefreshToken)
        {
            throw new ApiException(message: "Invalid refresh token provided.", errorCode: AppConstants.ErrorCodeEnum.InvalidRefreshToken);
        }

        var refrehTokenEntry = await this._refreshTokenEntryRepository.Get(rt => rt.Token == refreshTokenRequest.RefreshToken) 
            ?? throw new ApiException(message: "Refresh token has been expired.", errorCode: AppConstants.ErrorCodeEnum.InvalidRefreshToken);

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var refreshTokenEntries = await this.GetRefreshTokensByUserId(refrehTokenEntry.UserId);
        await this._refreshTokenEntryRepository.DeleteRange(refreshTokenEntries);

        var user = await this._userRepository.Get(u => u.Id == refrehTokenEntry.UserId);
        var tokenContainer = this._jwtService.GetJwtTokens(user);

        var newRefreshTokenEntry = new RefreshTokenEntry()
        {
            Token = tokenContainer.RefreshToken,
            UserId = refrehTokenEntry.UserId,
        };
        await this._refreshTokenEntryRepository.Add(newRefreshTokenEntry);

        scope.Complete();

        return tokenContainer;
    }

    public async Task CreateAccount(CreateAccountRequestModel createAccountRequest)
    {
        TempUser tempUser = await this.GetTempUser(createAccountRequest.UserEmail) ?? throw new Exception("User has not signed up");
        
        bool isValidSignupToken = this._jwtService.ValidateSignupToken(tempUser, createAccountRequest.SignupToken);
        if (!isValidSignupToken) 
        {
            throw new Exception("Invalid signup token provided.");
        }

        User newUser = this._mapper.Map<User>(createAccountRequest);
        newUser.AuthenticationHash = this._cryptographyService.Hash(createAccountRequest.AuthenticationKey);

        using TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await this._userRepository.Add(newUser);
        await this._tempUserRepository.Delete(tempUser);

        this._backgroundJobService.RegisterFireAndForgetJob<MailService>(ms => ms.SendAccountCreatedEmail(newUser.Email));

        scope.Complete();
    }

    public async Task Signup(SignupRequestModel signupRequest)
    {
        User user = await this.GetUser(signupRequest.UserEmail);
        if (user != null)
        {
            throw new ApiException(message: "User account already exists with this email.", errorCode: AppConstants.ErrorCodeEnum.Conflict);
        }

        TempUser tempUser = await this.GetTempUser(signupRequest.UserEmail) ?? await this.CreateTemporaryUser(signupRequest.UserEmail);

        string signupToken = this._jwtService.GenerateSignupToken(tempUser);

        this._backgroundJobService.RegisterFireAndForgetJob<MailService>(ms => ms.SendAccountActivationEmail(signupRequest.UserEmail, signupToken));
    }

    private async Task<User> GetUser(string userEmail)
    {
        return await this._userRepository.Get(u => u.Email == userEmail);
    }

    private async Task<TempUser> CreateTemporaryUser(string userEmail)
    {
        string uniqueUserId = this._cryptographyService.ConvertToUniqueId(userEmail);

        TempUser newUser = new()
        {
            Email = userEmail,
            HashCode = this._cryptographyService.Hash(uniqueUserId)
        };

        await this._tempUserRepository.Add(newUser);

        return newUser;
    }

    private async Task<TempUser> GetTempUser(string userEmail)
    {
        return await this._tempUserRepository.Get(tu => tu.Email == userEmail);
    }

    private async Task<IEnumerable<RefreshTokenEntry>> GetRefreshTokensByUserId(int userId)
    {
        return await _refreshTokenEntryRepository.GetList(predicate: rt => rt.UserId == userId);
    }
}

