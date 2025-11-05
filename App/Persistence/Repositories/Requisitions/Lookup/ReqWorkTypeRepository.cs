using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Features.Requisitions.Lookup;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories.Requisitions.Lookup;
public class ReqWorkTypeRepository(AppDbContext context) : IReqWorkTypeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionWorkType>> GetWorkTypes()
    {
        return await _context.RequisitionWorkType.ToListAsync();
    }
}