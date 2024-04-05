namespace SecurePass.Common.Models;

public class MailConfiguration
{
    public required string Mail { get; set; }

    public required string Username { get; set; }

    public required string DisplayName { get; set; }

    public required string Password { get; set; }

    public required string Host { get; set; }

    public int Port { get; set; }
}