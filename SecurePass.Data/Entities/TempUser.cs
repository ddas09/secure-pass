using System.ComponentModel.DataAnnotations;

namespace SecurePass.Data.Entities;

public class TempUser : BaseEntity
{
    [MaxLength(254)]
    public required string Email { get; set; }

    public required string HashCode { get; set; }
}

