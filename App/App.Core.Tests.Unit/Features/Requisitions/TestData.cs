using Application.Features.Requisitions.Commands.CreateRequisition;
using Domain.Entities.Requisitions;
using Moq;

namespace App.Core.Tests.Unit.Features.Requisitions;
public static class TestData
{
    public static Requisition CreateRequisition(CreateRequisitionCommand command = null)
    {
        command ??= CreateRequisitionCommand();
        return new Requisition()
        {
            Id = "600900",
            Name = "Senior Testing Engineer",
            StatusId = 1,
            WorkTypeId = 1,
            HardwareId = 1,
            Headcount = 2,
            Location = "Colombia",
            Department = "Engineering",
            HiringManager = "Don Juan",
            Owner = "HR Personnel",
            CompensationBandMin = 1000,
            CompensationBandMax = 2000,
            CompensationBandCurrency = "USD",
            CompensationBandInterval = "Annual",
            Description = "Requisition for a senior Engineer",
            BackfillName = "Alice Smith",
            Status = Mock.Of<RequisitionStatus>(s => s.Id == command.StatusId),
            WorkType = Mock.Of<RequisitionWorkType>(s => s.Id == command.WorkTypeId),
            Hardware = Mock.Of<RequisitionHardware>(s => s.Id == command.HardwareId),
            //Status = new RequisitionStatus { Id = 1, Status = "Open" },
            //WorkType = new RequisitionWorkType { Id = 1, WorkType = "FullTime" },
            //Hardware = new RequisitionHardware { Id = 1, Hardware = "Dell" },
        };
    }
    public static CreateRequisitionCommand CreateRequisitionCommand() => new()
    {
        Id = "600900",
        Name = "Senior Testing Engineer",
        StatusId = 1,
        WorkTypeId = 1,
        HardwareId = 1,
        Headcount = 2,
        Location = "Colombia",
        Department = "Engineering",
        HiringManager = "Don Juan",
        Owner = "HR Personnel",
        CompensationBandMin = 1000,
        CompensationBandMax = 2000,
        CompensationBandCurrency = "USD",
        CompensationBandInterval = "Annual",
        Description = "Requisition for a senior Engineer",
        BackfillName = "Alice Smith"
    };
}