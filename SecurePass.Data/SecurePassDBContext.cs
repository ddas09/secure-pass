using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SecurePass.Data.Configurations;

namespace SecurePass.Data;

public class SecurePassDBContext : DbContext
{
    public SecurePassDBContext(DbContextOptions<SecurePassDBContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<TempUser> TempUsers { get; set; }

    public DbSet<VaultRecord> VaultRecords { get; set; }

    public DbSet<SharedRecord> SharedRecords { get; set; }

    public DbSet<RecoverySetting> RecoverySettings { get; set; }

    public DbSet<RefreshTokenEntry> RefreshTokenEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TempUserConfiguration());
        modelBuilder.ApplyConfiguration(new VaultRecordConfiguration());
        modelBuilder.ApplyConfiguration(new SharedRecordConfiguration());
        modelBuilder.ApplyConfiguration(new RecoverySettingConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenEntryConfiguration());
    }
}

