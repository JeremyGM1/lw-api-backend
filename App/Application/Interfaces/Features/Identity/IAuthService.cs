using Application.Features.Identity.Models.AuthService;

namespace Application.Interfaces.Features.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> RefreshTokenAsync(TokenRefreshRequest request);
}