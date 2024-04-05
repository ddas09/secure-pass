namespace SecurePass.Common.Models;

public class UserModel
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string RSAPublicKey { get; set; }
}