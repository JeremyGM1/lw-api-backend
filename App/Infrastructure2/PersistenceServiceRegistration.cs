using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure2;
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }
}
