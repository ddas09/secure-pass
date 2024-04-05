using SecurePass.DAL.Contracts;
using SecurePass.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace SecurePass.DAL.Extensions;

public static class RepositoryCollection
{

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITempUserRepository, TempUserRepository>();
        services.AddScoped<IVaultRecordRepository, VaultRecordRepository>();
        services.AddScoped<ISharedRecordRepository, SharedRecordRepository>();
        services.AddScoped<IRecoverySettingRepository, RecoverySettingRepository>();
        services.AddScoped<IRefreshTokenEntryRepository, RefreshTokenEntryRepository>();
    }
}


