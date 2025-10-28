using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Lookup.Requisition;

namespace Persistence.Repositories.Lookup.Requisition;

public class ReqCurrencyRepository(AppDbContext context) : IReqCurrencyRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionCurrency>> GetCurrencies()
    {
        return await _context.RequisitionCurrency.ToListAsync();
    }
}