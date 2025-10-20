using Application.Common.Configs;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public UpdateRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    [Fact]
    public async Task Validate_WhenValidationSuccess_ShouldReturnSuccess()
    {
        var command = TestData.UpdateRequisitionCommand();
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WhenIdDoesNotExist_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Id = "600901";

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id))
            .ReturnsAsync((Requisition)null!);

        _mockReqRepository
           .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
           .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("does not exist");
    }

    [Theory]
    [InlineData("RequisitionName")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalName_ShouldReturnSuccess(string? name)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Name = name;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_ForeignKeysAreNotValid_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.StatusId = 777;
        command.WorkTypeId = 888;
        command.HardwareId = 999;

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(false);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(false);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(false);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "StatusId");
        result.Errors.First(e => e.PropertyName == "StatusId")
            .ErrorMessage.ShouldContain("does not exist");

        result.Errors.ShouldContain(e => e.PropertyName == "WorkTypeId");
        result.Errors.First(e => e.PropertyName == "WorkTypeId")
            .ErrorMessage.ShouldContain("does not exist");

        result.Errors.ShouldContain(e => e.PropertyName == "HardwareId");
        result.Errors.First(e => e.PropertyName == "HardwareId")
            .ErrorMessage.ShouldContain("does not exist");
    }

    [Fact]
    public async Task Validate_WhenHeadcountIsNegative_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Headcount = -1;

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Headcount");
        result.Errors.First(e => e.PropertyName == "Headcount")
            .ErrorMessage.ShouldContain("must be between 0 and");
    }

    [Theory]
    [InlineData("RequisitionLocation")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalLocation_ShouldReturnSuccess(string? location)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Location = location;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("RequisitionDepartment")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalDepartment_ShouldReturnSuccess(string? location)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Department = location;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("RequisitionHiringManager")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalHiringManager_ShouldReturnSuccess(string? hiringManager)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.HiringManager = hiringManager;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("Owner")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalOwner_ShouldReturnSuccess(string? owner)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Owner = owner;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("CompensationBandMin")]
    [InlineData("CompensationBandMax")]
    public async Task Validate_WhenCompensationBandsAreNegative_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.UpdateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, -1m);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
    }

    [Theory]
    [InlineData("CompensationBandMin")]
    [InlineData("CompensationBandMax")]
    public async Task Validate_WhenCompensationBandsAreAboveNumberLimit_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.UpdateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);

        const decimal MaxValueOverflow = ValidationDefaultValues.CompensationBandMaxValue + 1;
        property?.SetValue(command, MaxValueOverflow);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
    }

    [Fact]
    public async Task Validate_WhenCompensationBandCurrencyHasMoreThan3Characters_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.CompensationBandCurrency = "USDD";

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandCurrency");
        result.Errors.First(e => e.PropertyName == "CompensationBandCurrency")
            .ErrorMessage.ShouldContain("must be 3 characters or fewer");
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalCompensationBandCurrency_ShouldReturnSuccess(string? currency)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.CompensationBandCurrency = currency;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_WhenCompensationBandIntervalHasMoreThan10Characters_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.CompensationBandInterval = "MoreThan10Characters";

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandInterval");
        result.Errors.First(e => e.PropertyName == "CompensationBandInterval")
            .ErrorMessage.ShouldContain("must be 10 characters or fewer");
    }

    [Theory]
    [InlineData("Interval")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalCompensationBandInterval_ShouldReturnSuccess(string? compensationInterval)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.CompensationBandInterval = compensationInterval;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Description")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalDescription_ShouldReturnSuccess(string? description)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Description = description;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WhenDescriptionHasMoreThan200Characters_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.Description = new string('x', 201);
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
           .Setup(r => r.RequisitionExists(command.Id!))
           .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
        result.Errors.First(e => e.PropertyName == "Description")
            .ErrorMessage.ShouldContain("must be 200 characters or fewer");
    }

    [Theory]
    [InlineData("BackfillName")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_OptionalBackfillName_ShouldReturnSuccess(string? backfillName)
    {
        var command = TestData.UpdateRequisitionCommand();
        command.BackfillName = backfillName;
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_WhenBackfillNameHasMoreThan100Characters_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.BackfillName = new string('x', 101);
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
           .Setup(r => r.RequisitionExists(command.Id!))
           .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.Id!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "BackfillName");
        result.Errors.First(e => e.PropertyName == "BackfillName")
            .ErrorMessage.ShouldContain("must be 100 characters or fewer");
    }
}