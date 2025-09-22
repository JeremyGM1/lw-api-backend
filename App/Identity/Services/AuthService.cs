using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Application.Exceptions;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Identity.Models;

namespace Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            var response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email
            };

            return response;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
                return new RegistrationResponse() { UserId = user.Id };
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in result.Errors)
                {
                    str.AppendFormat("•{0}\n", err.Description);
                }
                throw new BadRequestException($"{str}");
            }
        }

        public async Task<UpdateResponse> Update(UpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id) ?? throw new NotFoundException($"User with {request.Id} not found.", request.Id);

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
                user.UserName = request.Email;
            }

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            await _userManager.UpdateAsync(user);

            return new UpdateResponse() { UserId = user.Id };
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User",null);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Unable to delete user: {errors}");
            }

            return true;
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
            }
            .Union(userClaims)
            .Union(roleClaims);

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
    }
}