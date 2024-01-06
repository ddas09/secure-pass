using System.ComponentModel.DataAnnotations.Schema;

namespace SecurePass.Data.Entities;

public class SharedRecord : BaseEntity
{
    [ForeignKey("VaultRecord")]
    public int RecordId { get; set; }

    public int OwnerId { get; set; }

    public int RecipientId { get; set; }

    public required string EncryptionKey { get; set; }

    public virtual VaultRecord VaultRecord { get; set; }

    public virtual User Owner { get; set; }

    public virtual User Recipient { get; set; }
}


