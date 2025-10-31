using Application.Common.Result;

namespace Application.Features.Requisitions.Errors;
public static class RequisitionErrors
{
    public static Error ExistingRequisition(string id) => new(
        "Requisitions.Existing",
        $"A requisition with this code '{id}' already exists");

    public static Error NotExistingRequisition(string id) => new(
        "Requisitions.NotExisting",
        $"A requisition with this code '{id}' does not exists");
}