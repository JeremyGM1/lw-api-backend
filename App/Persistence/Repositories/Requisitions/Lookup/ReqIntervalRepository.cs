using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Features.Requisitions.Lookup;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories.Requisitions.Lookup;
public class ReqIntervalRepository(AppDbContext context) : IReqIntervalRepository
{
    private readonly AppDbContext _context = context;
    public async Task<List<RequisitionInterval>> GetInterval()
    {
        return await _context.RequisitionInterval.ToListAsync();
    }
}