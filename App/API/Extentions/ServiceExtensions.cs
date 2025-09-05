using Application;
using FluentAssertions.Common;
using LoggingService;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extentions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                );
        });
        services.AddApplicationServices();
        services.AddLogginServices();
        services.AddPersistenceServices(configuration);
        services.AddCors(options =>
        {
            options.AddPolicy("all", builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Intership_backend", Version = "v1" });
        });
    }
}