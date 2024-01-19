using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class SigninRequestModel
{
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }

    [Required]
    public string AuthenticationKey { get; set; }
}

