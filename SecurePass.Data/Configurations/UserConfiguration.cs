using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
    }
}

