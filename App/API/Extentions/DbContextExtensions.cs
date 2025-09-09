using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extentions;

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
    }
}