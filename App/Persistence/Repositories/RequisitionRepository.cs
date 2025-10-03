using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Persistence.Repositories;
public class RequisitionRepository(AppDbContext context) : GenericRepository<Requisition, string>(context), IRequisitionRepository
{
}