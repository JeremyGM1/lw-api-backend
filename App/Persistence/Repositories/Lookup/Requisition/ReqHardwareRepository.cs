using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Lookup.Requisition;

namespace Persistence.Repositories.Lookup.Requisition;

public class ReqHardwareRepository(AppDbContext context) : IReqHardwareRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionHardware>> GetHardware()
    {
        return await _context.RequisitionHardware.ToListAsync();
    }
}