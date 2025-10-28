using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Lookup.Requisition;

namespace Persistence.Repositories.Lookup.Requisition;
public class ReqWorkTypeRepository(AppDbContext context) : IReqWorkTypeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionWorkType>> GetWorkTypes()
    {
        return await _context.RequisitionWorkType.ToListAsync();
    }
}