using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;
public interface IReqWorkTypeRepository
{
    Task<List<RequisitionWorkType>> GetWorkTypes();
}