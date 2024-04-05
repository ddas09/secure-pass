using SecurePass.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecurePass.Data.Configurations;

internal class SharedRecordConfiguration : BaseEntityConfiguration<SharedRecord>
{
    public override void Configure(EntityTypeBuilder<SharedRecord> builder)
    {
        base.Configure(builder);

        builder.HasOne(s => s.Owner)
                .WithMany(u => u.SharedRecordsAsOwner)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(s => s.Recipient)
                .WithMany(u => u.SharedRecordsAsRecipient)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}

