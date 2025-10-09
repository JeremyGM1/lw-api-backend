using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class RegistrationRequest
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(6)]
    public required string Password { get; set; }
    
    public string Role { get; set; } = string.Empty;
}