using Application.Interfaces;
using Application.Interfaces.Lookup.Requisition;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories.Lookup;
using Persistence.Repositories;
using Persistence.Repositories.Lookup.Requisition;

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
        return services;
    }
}