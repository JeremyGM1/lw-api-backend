using Application.Interfaces.Features.Lookup.Requisition;
using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Lookup.Requisition;
public class ReqIntervalRepository(AppDbContext context) : IReqIntervalRepository
{
    private readonly AppDbContext _context = context;
    public async Task<List<RequisitionInterval>> GetInterval()
    {
        return await _context.RequisitionInterval.ToListAsync();
    }
}