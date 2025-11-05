using System.ComponentModel.DataAnnotations;

namespace Application.Features.Identity.Models.UserService;

public class UpdateResponse
{
    [StringLength(36)]
    public string UserId { get; set; } = null!;
}