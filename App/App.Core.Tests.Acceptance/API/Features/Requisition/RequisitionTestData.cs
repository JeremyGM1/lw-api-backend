using Application.Features.Requisitions.Commands.CreateRequisition;

namespace App.Core.Tests.Acceptance.API.Features.Requisition;
public static class RequisitionTestData
{
    public static CreateRequisitionCommand GetValidCreateRequisitionCommand(string? reqCode = null)
    {
        return new CreateRequisitionCommand
        {
            ReqCode = reqCode,
            Name = "Test Requisition",
            StatusId = 1,
            WorkTypeId = 1,
            HardwareId = 1,
            LocationTypeId = 1,
            Headcount = 10,
            LocationId = 1,
            Department = "IT",
            HiringManager = "UserHiringManager",
            Owner = "UserRequisitionOwner",
            CompensationBandMin = 1000,
            CompensationBandMax = 5000,
            CompensationBandCurrencyId = 1,
            CompensationBandIntervalId = 1,
            Description = "ReqDescription",
            BackfillName = "ReqBackfillName"
        };
    }
}