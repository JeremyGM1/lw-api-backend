using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Features.JobDescriptions;
using Domain.Entities.JobDescriptions;

namespace Persistence.Repositories.JobDescriptions;

public class JobDescriptionRepository(AppDbContext context) : GenericRepository<JobDescription, int>(context), IJobDescriptionRepository
{
    public async Task<JobDescription?> GetJobDescriptionById(int id)
    {
        return await _context.JobDescriptions
            .Include(jd => jd.Requisition)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<JobDescription>> GetJobDescriptions()
    {
        return await _context.JobDescriptions
            .ToListAsync();
    }

    public async Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity != null;
    }
}