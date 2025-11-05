using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;

public interface IReqHardwareRepository
{
    Task<List<RequisitionHardware>> GetHardware();
}