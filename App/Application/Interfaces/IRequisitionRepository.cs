using Domain.Entities.Requisitions;

namespace Application.Interfaces;

public interface IRequisitionRepository : IGenericRepository<Requisition, string>
{
    Task<bool> ForeignKeyExist<TEntity>(object id) where TEntity : class;

    Task<Requisition?> GetRequisitionById(string id);

    Task<List<Requisition>> GetRequisitions();

    Task<bool> RequisitionExists(string id);
}