using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup.Requisition;
public interface IReqHardwareRepository
{
    Task<List<RequisitionHardware>> GetHardware();
}