using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class TempUserConfiguration : BaseEntityConfiguration<TempUser>
{
    public override void Configure(EntityTypeBuilder<TempUser> builder)
    {
        base.Configure(builder);
    }
}

