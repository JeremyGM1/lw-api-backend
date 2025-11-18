using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.LoggingService;
using LoggingService.Repositories;
using Domain.Entities.Requisitions;

namespace LoggingService;

public static class LogginServiceRegistration
{
    public static IServiceCollection AddLogginServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped(typeof(IGenericLogService<>), typeof(GenericLogService<>));
        return services;
    }
}