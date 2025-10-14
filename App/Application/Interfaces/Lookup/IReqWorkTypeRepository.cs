using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup;
public interface IReqWorkTypeRepository
{
    Task<List<RequisitionWorkType>> GetWorkTypes();
}