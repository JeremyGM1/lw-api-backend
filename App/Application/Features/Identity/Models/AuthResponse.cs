using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class AuthResponse
{
    [StringLength(50)]
    public string Id { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(2048)]
    public string Token { get; set; } = string.Empty;
}