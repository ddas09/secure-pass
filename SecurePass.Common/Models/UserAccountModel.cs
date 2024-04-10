namespace SecurePass.Common.Models;

public class UserModel
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string RSAPublicKey { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        UserModel other = (UserModel)obj;

        return Id == other.Id &&
            Email == other.Email &&
            Name == other.Name &&
            RSAPublicKey == other.RSAPublicKey;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Email, Name, RSAPublicKey);
    }
}