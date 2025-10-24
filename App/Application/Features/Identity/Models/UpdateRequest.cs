using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class UpdateRequest
{
    [Required]
    [StringLength(36)]
    public string Id { get; set; } = null!;

    [StringLength(254, MinimumLength = 5)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string? FirstName { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string? LastName { get; set; }

    [StringLength(128, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public string? NewPassword { get; set; }
}