using Domain.Entities.Requisitions;

namespace Application.Interfaces.Lookup;
public interface IReqHardwareRepository
{
    Task<List<RequisitionHardware>> GetHardware();
}