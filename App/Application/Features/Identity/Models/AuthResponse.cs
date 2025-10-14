using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class AuthResponse
{
    [StringLength(50)]
    [Required]
    public string Id { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(254)]
    [Required]
    public string Email { get; set; } = string.Empty;

    [StringLength(2048)]
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; }
}