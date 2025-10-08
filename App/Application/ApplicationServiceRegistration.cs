using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.Common.MappingProfiles;
using FluentValidation;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(UserProfile));
        services.AddAutoMapper(_ => { }, typeof(RequisitionProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}