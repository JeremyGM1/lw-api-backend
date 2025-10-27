using Application.Interfaces.Lookup.Requisition;
using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Lookup.Requisition;
public class ReqStatusRepository(AppDbContext context) : IReqStatusRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionStatus>> GetStatuses()
    {
        return await _context.RequisitionStatus.ToListAsync();
    }
}