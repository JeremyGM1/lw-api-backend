using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions.Lookup;

namespace Persistence.Repositories.Requisitions.Lookup;

public class ReqHardwareRepository(AppDbContext context) : IReqHardwareRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionHardware>> GetHardware()
    {
        return await _context.RequisitionHardware.ToListAsync();
    }
}