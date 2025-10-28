using Domain.Entities.Requisitions;

namespace Application.Interfaces.Features.Requisitions.Lookup;
public interface IReqIntervalRepository
{
    Task<List<RequisitionInterval>> GetInterval();
}