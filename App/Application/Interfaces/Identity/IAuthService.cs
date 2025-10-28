using Application.Features.Identity.Models;

namespace Application.Interfaces.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> RefreshTokenAsync(TokenRefreshRequest request);
}