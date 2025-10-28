using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;
public interface IReqCurrencyRepository
{
    Task<List<RequisitionCurrency>> GetCurrencies();
}