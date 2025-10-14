using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models;

public class RegistrationResponse
{
    [StringLength(36)]
    public string UserId { get; set; } = string.Empty;
}