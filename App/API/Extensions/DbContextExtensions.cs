using LoggingService;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class DbContextExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                );
        });

        services.AddDbContext<LoggingServiceDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                );
        });
    }
}