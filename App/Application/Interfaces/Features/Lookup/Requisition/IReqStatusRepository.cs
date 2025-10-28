using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;

public interface IReqStatusRepository
{
    Task<List<RequisitionStatus>> GetStatuses();
}