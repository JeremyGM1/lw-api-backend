using Application;
using LoggingService;
using Persistence;

namespace API.extensions;

public static class ServiceExtensions
{
    public static void AppServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddLogginServices();
        services.AddPersistenceServices(configuration);
    }
}