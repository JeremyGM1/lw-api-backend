using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.Shared.MappingProfiles;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(UserProfile));
        services.AddAutoMapper(_ => { }, typeof(RequisitionProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}