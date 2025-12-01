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
            .ThenInclude(r => r.Status)
            .Include(jd => jd.Requisition)
            .ThenInclude(r => r.LocationType)
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<JobDescription>> GetJobDescriptionsByRequisition(string reqCode)
    {
        return await _context.JobDescriptions
            .Include(jd => jd.Requisition)
            .ThenInclude(r => r.Status)
            .Include(jd => jd.Requisition)
            .ThenInclude(jd => jd.LocationType)
            .Where(jd => jd.RequisitionCode == reqCode)
            .ToListAsync();
    }

    public async Task<List<JobDescription>> GetJobDescriptions()
    {
        return await _context.JobDescriptions
            .Include(jd => jd.Requisition)
            .ThenInclude(r => r.Status)
            .Include(jd => jd.Requisition)
            .ThenInclude(r => r.LocationType)
            .ToListAsync();
    }

    public async Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity != null;
    }

    public async Task<List<JobDescriptionLog>> GetJDLogsById(int id)
    {
        return await _context.JobDescriptionLogs
            .Where(r => r.EntityId == id.ToString())
            .ToListAsync();
    }
}