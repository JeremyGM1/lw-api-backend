using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions;

namespace Persistence.Repositories.Requisitions;

public class RequisitionRepository(AppDbContext context) : GenericRepository<Requisition, int>(context), IRequisitionRepository
{
    public async Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity != null;
    }

    public async Task<Requisition?> GetRequisitionById(string reqCode)
    {
        return await _context.Requisitions
            .Include(r => r.Status)
            .Include(r => r.WorkType)
            .Include(r => r.Hardware)
            .Include(r => r.Location)
            .Include(r => r.LocationType)
            .Include(r => r.CompensationBandCurrency)
            .Include(r => r.CompensationBandInterval)
            .FirstOrDefaultAsync(r => r.ReqCode == reqCode);
    }

    public async Task<List<Requisition>> GetRequisitions()
    {
        return await _context.Requisitions
            .Include(r => r.Status)
            .Include(r => r.WorkType)
            .Include(r => r.Hardware)
            .Include(r => r.Location)
            .Include(r => r.LocationType)
            .Include(r => r.CompensationBandCurrency)
            .Include(r => r.CompensationBandInterval)
            .ToListAsync();
    }

    public async Task<bool> RequisitionExists(string reqCode)
    {
        return await _context.Requisitions
            .AnyAsync(r => r.ReqCode == reqCode);
    }
}