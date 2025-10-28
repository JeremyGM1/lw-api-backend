using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;
public interface IReqLocationRepository
{
    Task<List<RequisitionLocation>> GetLocations();
}