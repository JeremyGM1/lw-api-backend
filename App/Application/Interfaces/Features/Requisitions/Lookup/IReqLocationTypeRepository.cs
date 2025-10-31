using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;

public interface IReqLocationTypeRepository
{
    Task<List<RequisitionLocationType>> GetLocationTypes();
}