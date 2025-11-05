using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Features.Requisitions.Lookup;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories.Requisitions.Lookup;

public class ReqLocationRepository(AppDbContext context) : IReqLocationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionLocation>> GetLocations()
    {
        return await _context.RequisitionLocation.ToListAsync();
    }
}