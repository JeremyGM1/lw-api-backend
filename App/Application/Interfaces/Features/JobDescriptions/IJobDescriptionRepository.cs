using Domain.Entities.JobDescriptions;

namespace Application.Interfaces.Features.JobDescriptions;

public interface IJobDescriptionRepository : IGenericRepository<JobDescription, int>
{
    Task<JobDescription?> GetJobDescriptionById(int id);

    Task<List<JobDescription>> GetJobDescriptions();

    Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class;
}