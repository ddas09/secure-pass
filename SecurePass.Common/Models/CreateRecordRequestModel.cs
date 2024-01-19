using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class CreateRecordRequestModel
{
    [Required]
    public int OwnerId { get; set; }

    public string? Note { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Login { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string EncryptionKey { get; set; }

    public string? WebsiteUrl { get; set; }
}

