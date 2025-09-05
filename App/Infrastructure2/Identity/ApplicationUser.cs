using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infrastructure2.Identity;

public class ApplicationUser : IdentityUser
{
    public string DomainUserId { get; set; }
    public User DomainUser { get; set; } = null!;
}