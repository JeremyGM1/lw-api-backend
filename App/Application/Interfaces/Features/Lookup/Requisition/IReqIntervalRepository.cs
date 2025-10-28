using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;
public interface IReqIntervalRepository
{
    Task<List<RequisitionInterval>> GetInterval();
}