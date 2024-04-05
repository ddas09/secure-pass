using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SecurePass.API.Models;

public class CustomResponse
{
    private class ResponseModel
    {
        public dynamic? Data { get; set; }
        public required string Status { get; set; }
        public required string Message { get; set; }
        
        public override string ToString()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, serializeOptions);
        }
    }

    private static ContentResult BuildResponsePayload(HttpStatusCode statusCode, string status, string message, object? data = null)
    {
        var responseContent = new ResponseModel
        {
            Status = status,
            Message = message,
            Data = data
        };

        var response = new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = responseContent.ToString(),
            ContentType = AppConstants.ContentType
        };

        return response;
    }

    public ContentResult Success(string message, object data)
    {
        return BuildResponsePayload(HttpStatusCode.OK, AppConstants.Success, message, data);
    }

    public ContentResult Success(object data)
    {
        return BuildResponsePayload(HttpStatusCode.OK, AppConstants.Success, null, data);
    }

    public ContentResult Success(string message)
    {
        return BuildResponsePayload(HttpStatusCode.OK, AppConstants.Success, message);
    }

    public ContentResult Created(string message)
    {
        return BuildResponsePayload(HttpStatusCode.Created, AppConstants.Created, message);
    }

    public ContentResult Created(string message, object data)
    {
        return BuildResponsePayload(HttpStatusCode.Created, AppConstants.Created, message, data);
    }

    public ContentResult Conflict(string message)
    {
        return BuildResponsePayload(HttpStatusCode.Conflict, AppConstants.Conflict, message);
    }

    public ContentResult NotFound(string message)
    {
        return BuildResponsePayload(HttpStatusCode.NotFound, AppConstants.NotFound, message);
    }

    public ContentResult Forbidden(string message)
    {
        return BuildResponsePayload(HttpStatusCode.Forbidden, AppConstants.Forbidden, message);
    }

    public ContentResult BadRequest(string message)
    {
        return BuildResponsePayload(HttpStatusCode.BadRequest, AppConstants.BadRequest, message);
    }

    public ContentResult BadRequest(ModelStateDictionary modelState)
    {
        var errorMessage = modelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage).ToList().First();

        return BuildResponsePayload(HttpStatusCode.BadRequest, AppConstants.BadRequest, errorMessage);
    }

    public ContentResult Unauthorized(string message, string status = AppConstants.Unauthorized)
    {
        return BuildResponsePayload(HttpStatusCode.Unauthorized, status, message);
    }

    public ContentResult ServerError()
    {
        return BuildResponsePayload(HttpStatusCode.InternalServerError, AppConstants.ServerError, "Something went wrong. Internal server error!");
    }
}