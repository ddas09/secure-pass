using System.ComponentModel.DataAnnotations;

namespace SecurePass.Data.Entities;

public class User : BaseEntity
{
    [MaxLength(254)]
    public required string Email { get; set; }

    [MaxLength(100)]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    public required string LastName { get; set; }

    public required string AuthenticationHash { get; set; }

    public required string DataKey { get; set; }

    public required string RSAPublicKey { get; set; }

    public required string RSAPrivateKey { get; set; }

    public required string RandomSalt { get; set; }

    public virtual RecoverySetting RecoverySetting { get; set; }

    public virtual ICollection<VaultRecord> VaultRecords { get; set; }

    public virtual ICollection<SharedRecord> SharedRecordsAsOwner { get; set; }

    public virtual ICollection<SharedRecord> SharedRecordsAsRecipient { get; set; }
}

