namespace Application.Common.Results.RequisitionErrors;

public static class RequisitionErrors
{
    public static Error ExistingRequisition(string reqCode) => new(
        "Requisitions.Existing",
        $"A requisition with this code '{reqCode}' already exists");

    public static Error NotExistingRequisition(string id) => new(
        "Requisitions.NotExisting",
        $"A requisition with this code '{id}' does not exists");

    public static Error RequisitionException(string message) => new(
        "Requisitions.Exception",
        $"An error occurred while processing the requisition: {message}");

    public static Error RequisitionLogEmpty(string id) => new(
        "RequisitionLog.Empty",
        $"Requisition with id '{id}' doesn't have logs");
}