namespace SecurePass.Data.Entities;

public class RefreshTokenEntry : BaseEntity
{    
    public int UserId { get; set; }

    public required string Token { get; set; }

    public virtual User User { get; set; }
}

