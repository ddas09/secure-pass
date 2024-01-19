using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class CheckAccountExistenceRequestModel
{
    [Required]
    public required string UserEmail { get; set; }
}