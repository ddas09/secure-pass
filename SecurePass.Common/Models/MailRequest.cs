namespace SecurePass.Common.Models;

public class MailRequest
{
    public required string Subject { get; set; }

    public required string Body { get; set; }

    public required string RecipientEmail { get; set; }
}