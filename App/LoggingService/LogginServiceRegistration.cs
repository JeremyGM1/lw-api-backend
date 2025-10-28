using Microsoft.Extensions.DependencyInjection;
using LoggingService.Interfaces;
using LoggingService.Repositories;
using LoggingService.Repository;

namespace LoggingService;

public static class LogginServiceRegistration
{
    public static IServiceCollection AddLogginServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<IGenericLogService, GenericLogService>();
        services.AddScoped<IRequisitionLogService, RequisitionLogService>();
        return services;
    }
}