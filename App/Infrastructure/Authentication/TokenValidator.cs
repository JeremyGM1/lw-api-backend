using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;

namespace Infrastructure.Authentication;

public class TokenValidator(IConfiguration configuration) : ITokenValidator
{
    public bool ValidateToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };        

        try
        {            
            new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken ValidatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
