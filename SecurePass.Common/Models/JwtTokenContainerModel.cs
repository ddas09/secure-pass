namespace SecurePass.Common.Models;

public class JwtTokenContainerModel
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}