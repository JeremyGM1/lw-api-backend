using Application.Interfaces.Features.Requisitions.Lookup;
using Domain.Entities.Requisitions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Requisitions.Lookup;

public class ReqLocationTypeRepository(AppDbContext context) : IReqLocationTypeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionLocationType>> GetLocationTypes()
    {
        return await _context.RequisitionLocationType.ToListAsync();
    }
}