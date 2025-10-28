using Moq;
using Shouldly;
using Application.Common.Configs;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public UpdateRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    private void SetupForeignKeyExist<Type>(object id, bool exists) where Type : class
    {
        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<Type>(id))
            .ReturnsAsync(exists);
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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public async Task Validate_ForeignKeysAreNotValid_ShouldReturnFailure()
    {
        var command = TestData.UpdateRequisitionCommand();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id!))
            .ReturnsAsync(true);

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, false);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, false);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, false);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, false);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, false);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, false);

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "StatusId");
        result.Errors.First(e => e.PropertyName == "StatusId")
            .ErrorMessage.ShouldContain("does not exist");

        result.Errors.ShouldContain(e => e.PropertyName == "WorkTypeId");
        result.Errors.ShouldContain(e => e.PropertyName == "HardwareId");
        result.Errors.ShouldContain(e => e.PropertyName == "LocationId");
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandCurrencyId");
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandIntervalId");
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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId, true);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId, true);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId, true);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId, true);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId, true);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId, true);

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

        var validator = new UpdateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "BackfillName");
        result.Errors.First(e => e.PropertyName == "BackfillName")
            .ErrorMessage.ShouldContain("must be 100 characters or fewer");
    }
}