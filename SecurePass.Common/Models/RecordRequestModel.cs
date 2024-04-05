using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class BaseRecordRequestModel
{
    [Required]
    public int OwnerId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Login { get; set; }

    [Required]
    public required string Password { get; set; }

    public string? WebsiteUrl { get; set; }
}

public class CreateRecordRequestModel : BaseRecordRequestModel
{
    [Required]
    public required string EncryptionKey { get; set; }
}

public class UpdateRecordRequestModel : BaseRecordRequestModel
{
    [Required]
    public int Id { get; set; }
}
