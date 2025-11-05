using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;

public interface IReqLocationRepository
{
    Task<List<RequisitionLocation>> GetLocations();
}