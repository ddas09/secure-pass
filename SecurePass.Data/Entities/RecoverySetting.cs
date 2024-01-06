namespace SecurePass.Data.Entities;

public class RecoverySetting : BaseEntity
{    
    public int UserId { get; set; }

    public required string EncryptedSecurityQuestion { get; set; }

    public required string SecurityAnswerHash { get; set; }

    public required string RecoveryKey { get; set; }

    public virtual User User { get; set; }
}

