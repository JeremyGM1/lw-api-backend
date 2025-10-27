using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;
public interface IReqCurrencyRepository
{
    Task<List<RequisitionCurrency>> GetCurrencies();
}