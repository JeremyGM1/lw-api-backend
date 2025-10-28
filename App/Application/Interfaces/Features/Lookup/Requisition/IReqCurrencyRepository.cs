using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;
public interface IReqCurrencyRepository
{
    Task<List<RequisitionCurrency>> GetCurrencies();
}