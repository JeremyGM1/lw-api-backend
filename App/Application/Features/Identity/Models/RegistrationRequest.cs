using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class RegistrationRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public required string LastName { get; set; }

    [Required]
    [StringLength(254, MinimumLength = 5)]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
    public required string Password { get; set; }
    
    public string Role { get; set; } = string.Empty;
}