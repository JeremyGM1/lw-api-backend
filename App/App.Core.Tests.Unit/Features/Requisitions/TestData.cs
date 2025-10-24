using Moq;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions;

public static class TestData
{
    public static Requisition CreateRequisition(CreateRequisitionCommand command = null!)
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
            Location = Mock.Of<RequisitionLocation>(l => l.Id == command.LocationId),
            Department = "Engineering",
            HiringManager = "Don Juan",
            Owner = "HR Personnel",
            CompensationBandMin = 1000,
            CompensationBandMax = 2000,
            CompensationBandCurrency = Mock.Of<RequisitionCurrency>(l => l.Id == command.CompensationBandCurrencyId),
            CompensationBandInterval = Mock.Of<RequisitionInterval>(l => l.Id == command.CompensationBandIntervalId),
            Description = "Requisition for a senior Engineer",
            BackfillName = "Alice Smith",
            Status = Mock.Of<RequisitionStatus>(s => s.Id == command.StatusId),
            WorkType = Mock.Of<RequisitionWorkType>(s => s.Id == command.WorkTypeId),
            Hardware = Mock.Of<RequisitionHardware>(s => s.Id == command.HardwareId),
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
        LocationId = 1,
        Department = "Engineering",
        HiringManager = "Don Juan",
        Owner = "HR Personnel",
        CompensationBandMin = 1000,
        CompensationBandMax = 2000,
        CompensationBandCurrencyId = 1,
        CompensationBandIntervalId = 2,
        Description = "Requisition for a senior Engineer",
        BackfillName = "Alice Smith"
    };

    public static UpdateRequisitionCommand UpdateRequisitionCommand() => new()
    {
        Id = "600900",
        Name = "QA III",
        StatusId = 2,
        WorkTypeId = 2,
        HardwareId = 2,
        Headcount = 10,
        Location = "Philippines",
        Department = "Engineering",
        HiringManager = "Adrian",
        Owner = "Camilo",
        CompensationBandMin = 9000000,
        CompensationBandMax = 15000000,
        CompensationBandCurrency = "COP",
        CompensationBandInterval = "Weekly",
        Description = "No experience needed",
        BackfillName = "John Doe"
    };
}