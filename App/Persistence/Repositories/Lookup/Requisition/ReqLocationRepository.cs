using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions.Lookup;

namespace Persistence.Repositories.Lookup.Requisition;

public class ReqLocationRepository(AppDbContext context) : IReqLocationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionLocation>> GetLocations()
    {
        return await _context.RequisitionLocation.ToListAsync();
    }
}