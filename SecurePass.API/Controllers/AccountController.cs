using SecurePass.Services;
using SecurePass.API.Models;
using SecurePass.Common.Models;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Constants;
using SecurePass.API.ActionFilters;
using SecurePass.Services.Contracts;

namespace SecurePass.API.Controllers;

/// <summary>
/// Controller responsible for managing user accounts.
/// </summary>
/// <param name="accountService">The Account service for handling user account related operations.</param>
[ApiController]
[ValidateModelState]
[Route("api/accounts")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly CustomResponse _customResponse = new();
    private readonly IAccountService _accountService = accountService;
    private int CurrentUserId => int.Parse(JwtService.GetClaimValue(HttpContext.User, AppConstants.IdClaim));

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="signupRequest">The signup request containing user details.</param>
    [HttpPost("[action]")]
    public async Task<IActionResult> Signup(SignupRequestModel signupRequest)
    {
        await _accountService.Signup(signupRequest);

        var message = "Activation link sent successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Checks if the provided user email already exists in the system.
    /// </summary>
    /// <param name="request">The request containing the user email to check.</param>
    [HttpPost("check-existence")]
    public async Task<IActionResult> CheckExistence(CheckAccountExistenceRequestModel request)
    {
        var accountExists = await _accountService.AccountExists(request.UserEmail);
        return _customResponse.Success(data: accountExists);
    }

    /// <summary>
    /// Retrieves a unique salt value for password hashing.
    /// </summary>
    /// <param name="saltRequest">The request containing user information.</param>
    [HttpPost("salt")]
    public async Task<IActionResult> GetUniqueSalt(SaltRequestModel saltRequest)
    {
        var salt = await _accountService.GetSalt(saltRequest);
        return _customResponse.Success(data: salt);
    }

    /// <summary>
    /// Authenticates a user and generates an authentication token.
    /// </summary>
    /// <param name="signinRequest">The request containing user credentials.</param>
    [HttpPost("[action]")]
    public async Task<IActionResult> Signin(SigninRequestModel signinRequest)
    {
        var tokenContainer = await _accountService.Signin(signinRequest);
        return _customResponse.Success(data: tokenContainer);   
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <param name="createAccountRequest">The request containing user information.</param>
    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequestModel createAccountRequest)
    {
        await _accountService.CreateAccount(createAccountRequest);
        var message = "User account created successfully.";
        return _customResponse.Created(message: message);
    }

    /// <summary>
    /// Refreshes the authentication token.
    /// </summary>
    /// <param name="refreshTokenRequest">The request containing refresh token.</param>
    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel refreshTokenRequest)
    {
        var tokenContainer = await _accountService.RefreshTokens(refreshTokenRequest);
        return _customResponse.Success(data: tokenContainer);
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout(this.CurrentUserId);
        string message = "User account logged out successfully.";
        return _customResponse.Success(message: message);
    }

    /// <summary>
    /// Retrieves all users (admin access required).
    /// </summary>
    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await this._accountService.GetAllUsers(this.CurrentUserId);
        return _customResponse.Success(data: users);
    }
}
