namespace SecurePass.Data.Entities;

public class VaultRecord : BaseEntity
{
    public int OwnerId { get; set; }

    public required string Title { get; set; }

    public required string Login { get; set; }

    public required string Password { get; set; }

    public required string WebsiteUrl { get; set; }

    public required string Notes { get; set; }

    public required string EncryptionKey { get; set; }

    public virtual User Owner { get; set; }

    public virtual List<SharedRecord> SharedRecords { get; set; }
}


