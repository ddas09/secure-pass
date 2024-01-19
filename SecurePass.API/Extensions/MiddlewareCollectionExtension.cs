using SecurePass.API.Middlewares;

namespace SecurePass.API.Extensions;

public static class MiddlewareCollection
{
    public static void ConfigureMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}


