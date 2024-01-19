using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class SignupRequestModel
{
    [Required]
    [EmailAddress]
    public required string UserEmail { get; set; }
}

