using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Logging;

namespace LoggingService;
public static class LogginServiceRegistration
{
    public static IServiceCollection AddLogginServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}
