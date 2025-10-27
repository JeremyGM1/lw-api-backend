using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;
public interface IReqIntervalRepository
{
    Task<List<RequisitionInterval>> GetInterval();
}