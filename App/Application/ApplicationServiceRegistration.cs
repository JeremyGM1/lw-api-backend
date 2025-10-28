using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Application.Features.Users.MappingProfiles;
using Application.Features.Requisitions.MappingProfiles;
using Application.Common.Helpers;
using Application.Interfaces.Features.Common;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(UserProfile));
        services.AddAutoMapper(_ => { }, typeof(RequisitionProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<ICommonHelpers, CommonHelpers>();

        return services;
    }
}