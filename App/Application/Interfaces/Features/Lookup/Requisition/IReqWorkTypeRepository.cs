using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;
public interface IReqWorkTypeRepository
{
    Task<List<RequisitionWorkType>> GetWorkTypes();
}