using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Features.Requisitions.Lookup;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.Features.Users;
using Application.Interfaces.Features;
using Persistence.Repositories.Requisitions.Lookup;
using Persistence.Repositories.Requisitions;
using Persistence.Repositories.Users;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRequisitionRepository, RequisitionRepository>();
        services.AddScoped<IReqStatusRepository, ReqStatusRepository>();
        services.AddScoped<IReqWorkTypeRepository, ReqWorkTypeRepository>();
        services.AddScoped<IReqHardwareRepository, ReqHardwareRepository>();
        services.AddScoped<IReqLocationRepository, ReqLocationRepository>();
        services.AddScoped<IReqCurrencyRepository, ReqCurrencyRepository>();
        services.AddScoped<IReqIntervalRepository, ReqIntervalRepository>();
        services.AddScoped<IReqLocationTypeRepository, ReqLocationTypeRepository>();
        return services;
    }
}