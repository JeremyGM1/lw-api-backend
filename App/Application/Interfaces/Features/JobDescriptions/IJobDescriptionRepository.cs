using Domain.Entities.JobDescriptions;

namespace Application.Interfaces.Features.JobDescriptions;

public interface IJobDescriptionRepository : IGenericRepository<JobDescription, int>
{
    Task<JobDescription?> GetJobDescriptionById(int id);

    Task<List<JobDescription>> GetJobDescriptionsByRequisition(string reqCode);

    Task<List<JobDescription>> GetJobDescriptions();

    Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class;

    Task<List<JobDescriptionLog>> GetJDLogsById(int id);
}