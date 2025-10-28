using Microsoft.EntityFrameworkCore;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions.Lookup;

namespace Persistence.Repositories.Requisitions.Lookup;

public class ReqCurrencyRepository(AppDbContext context) : IReqCurrencyRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<RequisitionCurrency>> GetCurrencies()
    {
        return await _context.RequisitionCurrency.ToListAsync();
    }
}