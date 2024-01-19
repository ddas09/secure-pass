using SecurePass.API.ActionFilters;

namespace SecurePass.API.Extensions;

public static class ActionFilterCollection
{
    public static void RegisterActionFilters(this IServiceCollection services)
    {
        services.AddScoped<AuthorizeAttribute>();
        services.AddScoped<ValidateModelStateAttribute>();
    }
}


