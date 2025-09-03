using Microsoft.EntityFrameworkCore;
using Application;
using LoggingService;
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
    }
}