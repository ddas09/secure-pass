using System.ComponentModel.DataAnnotations;

namespace SecurePass.Data.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set;}

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}

