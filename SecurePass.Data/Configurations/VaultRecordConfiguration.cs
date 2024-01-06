using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class VaultRecordConfiguration : BaseEntityConfiguration<VaultRecord>
{
    public override void Configure(EntityTypeBuilder<VaultRecord> builder)
    {
        base.Configure(builder);
    }
}

