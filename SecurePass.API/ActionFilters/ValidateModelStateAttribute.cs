using SecurePass.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecurePass.API.ActionFilters;

public class ValidateModelStateAttribute : ActionFilterAttribute, IActionFilter
{
    private readonly CustomResponse _customResponse;

    public ValidateModelStateAttribute()
    {
        _customResponse = new CustomResponse();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = _customResponse.BadRequest(context.ModelState);
        }
    }
}

