using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class RecoverySettingConfiguration : BaseEntityConfiguration<RecoverySetting>
{
    public override void Configure(EntityTypeBuilder<RecoverySetting> builder)
    {
        base.Configure(builder);
    }
}

