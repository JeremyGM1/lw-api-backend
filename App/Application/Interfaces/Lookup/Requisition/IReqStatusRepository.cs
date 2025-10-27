using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;

public interface IReqStatusRepository
{
    Task<List<RequisitionStatus>> GetStatuses();
}