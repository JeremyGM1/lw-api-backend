using Application.Features.Identity.Models.UserService;
using Application.Interfaces.Features.Identity;
using Identity.Configurations;
using Identity.DbContext;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity;

public static class IdentityServicesRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<AdminSettings>(configuration.GetSection("AdminSettings"));
        services.AddDbContext<TalentSuiteIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<TalentSuiteIdentityDbContext>()
            .AddDefaultTokenProviders();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();

        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer( o => {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:key"]))
            };
        });

        return services;
    }
}