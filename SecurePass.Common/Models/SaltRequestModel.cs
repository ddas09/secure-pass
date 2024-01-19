using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class SaltRequestModel
{
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
}

