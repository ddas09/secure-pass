using SecurePass.API.Models;
using System.Security.Claims;
using SecurePass.Common.Models;
using Microsoft.AspNetCore.Mvc;
using SecurePass.API.ActionFilters;
using SecurePass.Services.Contracts;

namespace SecurePass.API.Controllers;

[ApiController]
[ValidateModelState]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly CustomResponse _customResponse;
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _logger = logger;
        _accountService = accountService;
        _customResponse = new CustomResponse();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Signup(SignupRequestModel signupRequest)
    {
        _logger.LogInformation("Signup endpoint of account controller invoked.");

        await _accountService.Signup(signupRequest);

        var message = "Activation link sent successfully.";
        _logger.LogInformation(message);

        return _customResponse.Success(message: message);
    }

    [HttpPost("check-existence")]
    public async Task<IActionResult> CheckExistence(CheckAccountExistenceRequestModel request)
    {
        _logger.LogInformation("CheckExistence endpoint of account controller invoked.");

        var accountExists = await _accountService.AccountExists(request.UserEmail);

        return _customResponse.Success(data: accountExists);
    }

    [HttpPost("salt")]
    public async Task<IActionResult> GetUniqueSalt(SaltRequestModel saltRequest)
    {
        _logger.LogInformation("GetUniqueSalt endpoint of account controller invoked.");

        var salt = await _accountService.GetSalt(saltRequest);
        _logger.LogInformation("User account salt retrieved.");

        return _customResponse.Success(data: salt);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Signin(SigninRequestModel signinRequest)
    {
        _logger.LogInformation("Signin endpoint of account controller invoked.");

        var tokenContainer = await _accountService.Signin(signinRequest);
        _logger.LogInformation("Jwt tokens generated for user.");

        return _customResponse.Success(tokenContainer);   
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequestModel createAccountRequest)
    {
        _logger.LogInformation("CreateAccount endpoint of account controller invoked.");

        await _accountService.CreateAccount(createAccountRequest);

        var message = "User account created successfully.";
        _logger.LogInformation(message);

        return _customResponse.Created(message);
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel refreshTokenRequest)
    {
        _logger.LogInformation("RefreshAccessToken endpoint of account controller invoked.");

        var tokenContainer = await _accountService.RefreshTokens(refreshTokenRequest);
        _logger.LogInformation("Jwt tokens refreshed for user.");

        return _customResponse.Success(data: tokenContainer);
    }

    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Logout endpoint of account controller invoked.");

        int userId = int.Parse(HttpContext.User.FindFirstValue("id"));
        await _accountService.Logout(userId);

        string message = "User account logged out successfully.";
        _logger.LogInformation(message);

        return _customResponse.Success(data: message);
    }
}
