using Application.Common.Configs;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces.Features.Requisitions;
using Domain.Entities.Requisitions;
using Moq;
using Shouldly;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly CreateRequisitionCommandValidator _validator;

    public CreateRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);
    }

    private void SetupAllForeignKeys()
    {
        _mockReqRepository
            .Setup(r => r.ForeignKeyExist<It.IsAnyType>(It.IsAny<int>()))
            .ReturnsAsync(true);
    }

    #region ReqCode Tests

    [Fact]
    public async Task CreateValidator_WhenReqCodeHas6Chars_ShouldPass()
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = "600900";
        SetupAllForeignKeys();

        var result = await _validator.ValidateAsync(command);

        result.Errors.ShouldNotContain(e => e.PropertyName == "ReqCode");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateValidator_WhenReqCodeIsEmptyOrNull_ShouldFail(string? data)
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = data!;
        SetupAllForeignKeys();

        var result = await _validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
    }

    [Fact]
    public async Task CreateValidation_WhenReqCodeHasMoreThan6Chars_ShouldFail()
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = "6009011";
        SetupAllForeignKeys();

        var result = await _validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
    }

    [Fact]
    public async Task CreateValidation_WhenReqCodeHasLessThan6Chars_ShouldFail()
    {
        var command = TestData.CreateRequisitionCommand();
        command.ReqCode = "60090";
        SetupAllForeignKeys();

        var result = await _validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
    }

    #endregion

    #region Required Strings Tests    

    [Theory]
    [InlineData("Name")]
    [InlineData("Department")]
    [InlineData("HiringManager")]
    [InlineData("Owner")]
    public async Task CreateValidator_WhenRequiredStringsAreNull_ShouldFail(string propertyName)
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

    #endregion

    #region Foreign Keys Tests

    [Theory]
    [InlineData("StatusId")]
    [InlineData("WorkTypeId")]
    [InlineData("HardwareId")]
    [InlineData("LocationId")]
    [InlineData("LocationTypeId")]
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
    [InlineData("StatusId")]
    [InlineData("WorkTypeId")]
    [InlineData("HardwareId")]
    [InlineData("LocationId")]
    [InlineData("LocationTypeId")]
    [InlineData("CompensationBandCurrencyId")]
    [InlineData("CompensationBandIntervalId")]
    public async Task CreateValidator_WhenForeignKeysAreHigherThanMaxValue_ShouldReturnFailure(string propertyName)
    {
        var command = TestData.CreateRequisitionCommand();
        var property = command.GetType().GetProperty(propertyName);
        property?.SetValue(command, ValidationDefaultValues.IntMaxValue);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == propertyName);
        result.Errors.First(e => e.PropertyName == propertyName)
            .ErrorMessage.ShouldContain("does not exist");
    }

    #endregion

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
}