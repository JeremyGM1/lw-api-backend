using Application.Interfaces.Lookup;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories.Lookup;
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
        return services;
    }
}