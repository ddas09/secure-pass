using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class UpdateRecordRequestModel
{
    [Required]
    public int OwnerId { get; set; }

    [Required]
    public int RecordId { get; set; }

    public string Note { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }

    public string WebsiteUrl { get; set; }
}

