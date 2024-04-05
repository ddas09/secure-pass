using System.ComponentModel.DataAnnotations;

namespace SecurePass.Common.Models;

public class CreateAccountRequestModel
{
    [Required]
    [EmailAddress]
    public required string UserEmail { get; set; }

    [Required]
    public required string DataKey { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string RandomSalt { get; set; }

    [Required]
    public required string RSAPublicKey { get; set; }

    [Required]
    public required string RSAPrivateKey { get; set; }

    [Required]
    public required string AuthenticationKey { get; set; }

    [Required]
    public required string SignupToken { get; set; }
}

