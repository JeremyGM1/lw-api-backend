using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.LoggingService;
using LoggingService.Repositories;

namespace LoggingService;

public static class LogginServiceRegistration
{
    public static IServiceCollection AddLogginServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<ILoggerService, GenericLogService>();
        return services;
    }
}