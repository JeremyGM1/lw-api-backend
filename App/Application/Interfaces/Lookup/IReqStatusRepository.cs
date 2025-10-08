using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup;

public interface IReqStatusRepository
{
    Task<List<RequisitionStatus>> GetStatuses();
}