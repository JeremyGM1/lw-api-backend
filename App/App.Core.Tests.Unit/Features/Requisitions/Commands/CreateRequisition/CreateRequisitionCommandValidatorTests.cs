using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces.Features.Requisitions;
using Domain.Entities.Requisitions;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public CreateRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    private void SetupForeignKeyExist<Type>(object id) where Type : class
    {
        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<Type>(id))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task CreateValidator_WhenIdAlreadyExist_ShouldReturnValidationError()
    {
        var command = TestData.CreateRequisitionCommand();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.ReqCode))
            .ReturnsAsync(true);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("already exist.");
    }

    [Fact]
    public async Task CreateValidator_WhenCommandIsValid_ShouldReturnSuccess()
    {
        var command = TestData.CreateRequisitionCommand();
        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.ReqCode))
            .ReturnsAsync(false);

        SetupForeignKeyExist<RequisitionStatus>(command.StatusId);
        SetupForeignKeyExist<RequisitionWorkType>(command.WorkTypeId);
        SetupForeignKeyExist<RequisitionHardware>(command.HardwareId);
        SetupForeignKeyExist<RequisitionLocation>(command.LocationId);
        SetupForeignKeyExist<RequisitionCurrency>(command.CompensationBandCurrencyId);
        SetupForeignKeyExist<RequisitionInterval>(command.CompensationBandIntervalId);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null, "is required")]
    [InlineData("", "must not be empty")]
    [InlineData("   ", "must not be empty")]
    public async Task CreateValidator_WhenIdIsNullOrEmpty_ShouldReturnFailure(string? reqCode, string expectedError)
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = reqCode!;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("60040", "must be at least 6 characters")]
    [InlineData("6004044", "must be 6 characters or fewer")]
    public async Task CreateValidator_WhenIdLengthIsDifferentThan6_ShouldReturnFailure(string reqCode, string expectedError)
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = reqCode;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("StatusId")]
    [InlineData("WorkTypeId")]
    [InlineData("HardwareId")]
    [InlineData("LocationId")]
    [InlineData("CompensationBandCurrencyId")]
    [InlineData("CompensationBandIntervalId")]
    public async Task CreateValidator_WhenForeignKeysAreNull_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.CreateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, null);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
        result.Errors.First(e => e.PropertyName == propertyName)
            .ErrorMessage.ShouldContain("does not exist");
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Headcount")]
    [InlineData("Department")]
    [InlineData("HiringManager")]
    [InlineData("Owner")]
    public async Task CreateValidator_WhenRequiredPropertiesAreNull_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.CreateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, null);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Department")]
    [InlineData("HiringManager")]
    [InlineData("Owner")]
    public async Task CreateValidator_WhenRequiredPropertiesAreEmpty_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.CreateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, "");

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
    }

    [Fact]
    public async Task CreateValidator_WhenHeadcountIsLowerThan0_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.Headcount = -1;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Headcount");
        result.Errors.First(e => e.PropertyName == "Headcount")
            .ErrorMessage.ShouldContain("'Headcount' must be between 0 and");
    }

    [Theory]
    [InlineData("CompensationBandMin")]
    [InlineData("CompensationBandMax")]
    public async Task CreateValidator_WhenCurrencyBandsIsLowerThan0_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.CreateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, -100m);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
    }

    [Fact]
    public async Task CreateValidator_WhenDescriptionHasMoreThan200Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.Description = new string('x', 201);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task CreateValidator_WhenBackfillNameHasMoreThan100Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.BackfillName = new string('x', 101);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "BackfillName");
    }

    [Fact]
    public async Task CreateValidator_WhenReqWithSameIdExist_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = "600900";

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.ReqCode))
            .ReturnsAsync(true);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("already exist");
    }
}