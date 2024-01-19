namespace SecurePass.Common.Models;

public class JwtConfiguration
{
    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public int SignupTokenExpirationTimeInMinutes { get; set; }

    public required string AccessTokenSecret { get; set; }

    public int AccessTokenExpirationTimeInMinutes { get; set; }

    public required string RefreshTokenSecret { get; set; }

    public int RefreshTokenExpirationTimeInMinutes { get; set; }

    public int RecoveryTokenExpirationTimeInMinutes { get; set; }

    public required string IdentityTokenSecret { get; set; }

    public int IdentityTokenExpirationTimeInMinutes { get; set; }
}