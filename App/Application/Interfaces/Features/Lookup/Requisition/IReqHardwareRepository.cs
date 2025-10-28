using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Lookup.Requisition;
public interface IReqHardwareRepository
{
    Task<List<RequisitionHardware>> GetHardware();
}