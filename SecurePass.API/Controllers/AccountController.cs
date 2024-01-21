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
        await _accountService.Signup(signupRequest);

        var message = "Activation link sent successfully.";
        return _customResponse.Success(message: message);
    }

    [HttpPost("check-existence")]
    public async Task<IActionResult> CheckExistence(CheckAccountExistenceRequestModel request)
    {
        var accountExists = await _accountService.AccountExists(request.UserEmail);
        return _customResponse.Success(data: accountExists);
    }

    [HttpPost("salt")]
    public async Task<IActionResult> GetUniqueSalt(SaltRequestModel saltRequest)
    {
        var salt = await _accountService.GetSalt(saltRequest);
        return _customResponse.Success(data: salt);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Signin(SigninRequestModel signinRequest)
    {
        var tokenContainer = await _accountService.Signin(signinRequest);
        return _customResponse.Success(tokenContainer);   
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequestModel createAccountRequest)
    {
        await _accountService.CreateAccount(createAccountRequest);
        var message = "User account created successfully.";
        return _customResponse.Created(message);
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel refreshTokenRequest)
    {
        var tokenContainer = await _accountService.RefreshTokens(refreshTokenRequest);
        return _customResponse.Success(data: tokenContainer);
    }

    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> Logout()
    {
        int userId = int.Parse(HttpContext.User.FindFirstValue("id"));
        await _accountService.Logout(userId);

        string message = "User account logged out successfully.";
        return _customResponse.Success(data: message);
    }
}
