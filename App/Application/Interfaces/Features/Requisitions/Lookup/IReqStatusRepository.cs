using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;

public interface IReqStatusRepository
{
    Task<List<RequisitionStatus>> GetStatuses();
}