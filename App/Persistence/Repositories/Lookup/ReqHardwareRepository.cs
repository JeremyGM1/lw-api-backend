using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Lookup;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories.Lookup;

public class ReqHardwareRepository(AppDbContext context) : IReqHardwareRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionHardware>> GetHardware()
    {
        return await _context.RequisitionHardware.ToListAsync();
    }
}