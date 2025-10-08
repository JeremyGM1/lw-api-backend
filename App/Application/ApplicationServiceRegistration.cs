using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.Common.MappingProfiles;
using FluentValidation;
using Application.Features.Common.HandlerHelpers;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(UserProfile));
        services.AddAutoMapper(_ => { }, typeof(RequisitionProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<CommonHelpers>();

        return services;
    }
}