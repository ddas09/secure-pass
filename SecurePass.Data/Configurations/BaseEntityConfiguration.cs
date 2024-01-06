using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("getdate()");
        builder.Property(x => x.UpdatedDate).HasDefaultValueSql("getdate()");
    }
}

