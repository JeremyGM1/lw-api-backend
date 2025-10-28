using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;
public interface IReqLocationRepository
{
    Task<List<RequisitionLocation>> GetLocations();
}