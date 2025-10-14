using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories;

public class RequisitionRepository(AppDbContext context) : GenericRepository<Requisition, string>(context), IRequisitionRepository
{
    public async Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity != null;
    }

    public async Task<Requisition?> GetRequisitionById(string id)
    {
        return await _context.Requisitions
            .Include(r => r.Status)
            .Include(r => r.WorkType)
            .Include(r => r.Hardware)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Requisition>> GetRequisitions()
    {
        return await _context.Requisitions
            .Include(r => r.Status)
            .Include(r => r.WorkType)
            .Include(r => r.Hardware)
            .ToListAsync();
    }

    public async Task<bool> RequisitionExists(string id)
    {
        return await _context.Requisitions
            .AnyAsync(r => r.Id == id);
    }
}