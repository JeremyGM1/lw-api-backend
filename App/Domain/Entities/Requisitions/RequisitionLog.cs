using Domain.Common;

namespace Domain.Entities.Requisitions;

public class RequisitionLog : EntityLog
{
    public virtual Requisition Requisition { get; set; } = null!;
}