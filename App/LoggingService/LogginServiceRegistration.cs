using Microsoft.Extensions.DependencyInjection;
using LoggingService.Repository;
using LoggingService.Interfaces;

namespace LoggingService;

public static class LogginServiceRegistration
{
    public static IServiceCollection AddLogginServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}