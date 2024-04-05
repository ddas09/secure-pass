using SecurePass.Common.Models;

namespace SecurePass.Services.Contracts;

public interface IAccountService
{
    Task<List<UserModel>> GetAllUsers(int userId);

    Task Logout(int userId);

    Task Signup(SignupRequestModel signupRequest);

    Task<bool> AccountExists(string userEmail);

    Task<string> GetSalt(SaltRequestModel saltRequest);

    Task<JwtTokenContainerModel> Signin(SigninRequestModel signinRequest);

    Task CreateAccount(CreateAccountRequestModel createAccountRequest);

    Task<JwtTokenContainerModel> RefreshTokens(RefreshTokenRequestModel refreshTokenRequest);
}

