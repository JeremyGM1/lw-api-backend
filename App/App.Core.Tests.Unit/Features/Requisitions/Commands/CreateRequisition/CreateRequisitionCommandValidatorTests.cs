using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces;
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

    [Fact]
    public async Task Validator_WhenIdAlreadyExist_ShouldReturnValidationError()
    {
        var command = TestData.CreateRequisitionCommand();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(true);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldBe("Requisition with this Id already exist.");
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldReturnSuccess()
    {
        var command = TestData.CreateRequisitionCommand();
        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(false);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionStatus>(command.StatusId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionWorkType>(command.WorkTypeId))
            .ReturnsAsync(true);

        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<RequisitionHardware>(command.HardwareId))
            .ReturnsAsync(true);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null, "Id is required")]
    [InlineData("", "'Id' must not be empty")]
    [InlineData("   ", "'Id' must not be empty")]
    public async Task Validator_WhenIdIsNullOrEmpty_ShouldReturnFailure(string? id, string expectedError)
    {
        var command = TestData.CreateRequisitionCommand();
        command.Id = id!;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("60040", "must be at least 6 characters")]
    [InlineData("6004044", "must be 6 characters or fewer")]
    public async Task Validator_WhenIdLengthIsDifferentThan6_ShouldReturnFailure(string id, string expectedError)
    {
        var command = TestData.CreateRequisitionCommand();
        command.Id = id;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("StatusId")]
    [InlineData("WorkTypeId")]
    [InlineData("HardwareId")]
    public async Task Validator_WhenForeignKeysAreNull_ShouldReturnFailure(string propertyName)
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
    [InlineData("Location")]
    [InlineData("Department")]
    [InlineData("HiringManager")]
    [InlineData("Owner")]
    [InlineData("CompensationBandCurrency")]
    [InlineData("CompensationBandInterval")]
    public async Task Validator_WhenRequiredPropertiesAreNull_ShouldReturnFailure(string propertyName)
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
    [InlineData("Location")]
    [InlineData("Department")]
    [InlineData("HiringManager")]
    [InlineData("Owner")]
    [InlineData("CompensationBandCurrency")]
    [InlineData("CompensationBandInterval")]
    public async Task Validator_WhenRequiredPropertiesAreEmpty_ShouldReturnFailure(string propertyName)
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
    public async Task Validator_WhenHeadcountIsLowerThan0_ShouldReturnFailure()
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
    public async Task Validator_WhenCurrencyBandsIsLowerThan0_ShouldReturnFailure(string propertyName)
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
    public async Task Validator_WhenBandCurrencyHasMoreThan3Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.CompensationBandCurrency = "USDD";

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandCurrency");
    }

    [Fact]
    public async Task Validator_WhenBandIntervalyHasMoreThan10Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.CompensationBandCurrency = "USDD";

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "CompensationBandCurrency");
    }

    [Fact]
    public async Task Validator_WhenDescriptionHasMoreThan200Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.Description = new string('x', 201);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validator_WhenBackfillNameHasMoreThan100Characters_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.BackfillName = new string('x', 101);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "BackfillName");
    }

    [Fact]
    public async Task Validator_WhenReqWithSameIdExist_ShouldReturnFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        command.Id = "600900";

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(true);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("already exist");
    }
}