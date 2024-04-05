using System.Net;
using SecurePass.API.Models;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Constants;
using SecurePass.Common.Exceptions;

namespace SecurePass.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception exception) 
        {
            this._logger.LogError(exception.ToString());
            await this.HandleException(context: context, exception: exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var response = new CustomResponse();

        ContentResult result = exception switch
        {
            ApiException apiException => this.HandleApiException(apiException, response),
            _ => response.ServerError()
        };

        context.Response.StatusCode = result.StatusCode ?? (int)HttpStatusCode.OK;
        await context.Response.WriteAsync(result.Content ?? string.Empty);
    }

    private ContentResult HandleApiException(ApiException apiException, CustomResponse response)
    {
        return apiException.ErrorCode switch
        {
            AppConstants.ErrorCodeEnum.Conflict => response.Conflict(message: apiException.Message),
            AppConstants.ErrorCodeEnum.NotFound => response.NotFound(message: apiException.Message),
            AppConstants.ErrorCodeEnum.BadRequest => response.BadRequest(message: apiException.Message),
            AppConstants.ErrorCodeEnum.Unauthorized => response.Unauthorized(message: apiException.Message),
            AppConstants.ErrorCodeEnum.InvalidAccessToken => response.Unauthorized(message: apiException.Message, status: AppConstants.InvalidAccessToken),
            AppConstants.ErrorCodeEnum.InvalidRefreshToken => response.Unauthorized(message: apiException.Message, status: AppConstants.InvalidRefreshToken),
            _ => response.ServerError()
        };
    }
}

