using Application.Interfaces.Features.Requisitions.Lookup;
using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Requisitions.Lookup;
public class ReqStatusRepository(AppDbContext context) : IReqStatusRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionStatus>> GetStatuses()
    {
        return await _context.RequisitionStatus.ToListAsync();
    }
}