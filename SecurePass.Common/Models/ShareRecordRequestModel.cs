using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class ShareRecordRequestModel
{
    [Required]
    public int RecordId { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [Required]
    public int RecipientId { get; set; }

    [Required]
    public required string EncryptionKey { get; set; }
}

