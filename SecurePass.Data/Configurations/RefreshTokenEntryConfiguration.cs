using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class RefreshTokenEntryConfiguration : BaseEntityConfiguration<RefreshTokenEntry>
{
    public override void Configure(EntityTypeBuilder<RefreshTokenEntry> builder)
    {
        base.Configure(builder);
    }
}

