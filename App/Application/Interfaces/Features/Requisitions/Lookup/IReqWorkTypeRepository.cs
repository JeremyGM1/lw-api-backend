using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;
public interface IReqWorkTypeRepository
{
    Task<List<RequisitionWorkType>> GetWorkTypes();
}