using SecurePass.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace SecurePass.Services.Extensions;

public static class ServiceCollection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IRecordService, RecordService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
    }
}


