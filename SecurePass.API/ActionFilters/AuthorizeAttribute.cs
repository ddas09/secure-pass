using SecurePass.API.Models;
using SecurePass.Common.Constants;
using SecurePass.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecurePass.API.ActionFilters;

public class AuthorizeAttribute : ActionFilterAttribute, IActionFilter
{
    private readonly CustomResponse _customResponse;

    public AuthorizeAttribute()
    {
        _customResponse = new CustomResponse();
    }

    override public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AppConstants.AuthorizationHeaderKey, out var tokenData))
        {
            context.Result = _customResponse.Unauthorized("Access token is required.", AppConstants.TokenRequired);
            return;
        }

        var jwtToken = tokenData.First().Replace(AppConstants.BearerKey, string.Empty);
        var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>()
            ?? throw new InvalidOperationException($"Could not get service of type {nameof(IJwtService)}");

        if (!jwtService.ValidateAccessToken(jwtToken))
        {
            context.Result = _customResponse.Unauthorized("Invalid access token provided.", AppConstants.InvalidAccessToken);
        }
    }
}

