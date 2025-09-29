namespace Application.Features.Identity.Models;

public class UpdateRequest
{
    public string Id { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}