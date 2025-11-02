using Application.Exceptions;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Identity.DbContext;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly TalentSuiteIdentityDbContext _context;
    private readonly IPermissionService _permissionService;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings, SignInManager<ApplicationUser> signInManager, TalentSuiteIdentityDbContext context, IPermissionService permissionService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
        _context = context;
        _permissionService = permissionService;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new NotFoundException($"User with {request.Email} not found.", request.Email);
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
        var jwtId = jwtSecurityToken.Id;

        var refreshToken = GenerateRefreshToken(user.Id, jwtId);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        var response = new AuthResponse
        {
            Id = user.Id,
            Email = user.Email!,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            RefreshToken = refreshToken.Token
        };

        return response;
    }

    public async Task<AuthResponse> RefreshTokenAsync(TokenRefreshRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        var userId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier ||x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
            throw new SecurityTokenException("Invalid refresh token");

        if (storedRefreshToken.JwtId != jti)
            throw new SecurityTokenException("Token mismatch");

        if (storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
            throw new SecurityTokenException("Refresh token expired");

        storedRefreshToken.IsUsed = true;
        await _context.SaveChangesAsync();

        await CleanOldRefreshTokensAsync(userId!);

        var user = await _userManager.FindByIdAsync(userId!);
        var newJwt = await GenerateToken(user!);
        var newRefresh = GenerateRefreshToken(user!.Id, newJwt.Id);

        _context.RefreshTokens.Add(newRefresh);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Id = user.Id,
            Email = user.Email!,
            Token = new JwtSecurityTokenHandler().WriteToken(newJwt),
            RefreshToken = newRefresh.Token
        };
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);
        var permissionClaims = permissions.Select(p => new Claim("permission", p.Name)).ToList();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        }
        .Union(roleClaims)
        .Union(permissionClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken(string userId, string jwtId)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var refreshTokenValue = Convert.ToBase64String(randomBytes);

        return new RefreshToken
        {
            Token = refreshTokenValue,
            JwtId = jwtId,
            UserId = userId,
            IsUsed = false,
            IsRevoked = false,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };
    }

    private async Task CleanOldRefreshTokensAsync(string userId)
    {
        var expirationThreshold = DateTime.UtcNow.AddDays(-_jwtSettings.CleanupThresholdInDays);

        var oldTokens = _context.RefreshTokens
            .Where(rt =>
                rt.UserId == userId &&
                (rt.IsRevoked || rt.ExpiryDate < expirationThreshold));

        _context.RefreshTokens.RemoveRange(oldTokens);
        await _context.SaveChangesAsync();
    }

}