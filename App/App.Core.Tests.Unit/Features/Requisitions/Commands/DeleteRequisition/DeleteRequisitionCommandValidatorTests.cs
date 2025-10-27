using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Interfaces;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public DeleteRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    [Fact]
    public async Task Validator_WhenIdExist_ShouldDeleteRequisition()
    {
        var command = new DeleteRequisitionCommand { Id = "600900" };

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(true);

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task ValidatorWhenIdDoesNotExist_ShouldReturnBadRequestException()
    {
        var command = new DeleteRequisitionCommand { Id = "600901" };

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(false);

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("Id does not exist");
    }

    [Fact]
    public async Task Validator_WhenIdIsNull_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { Id = null! };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("Id is required");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Validator_WhenIdIsEmpty_ShouldReturnFailure(string propertyValue)
    {
        var command = new DeleteRequisitionCommand { Id = propertyValue };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("must not be empty");
    }

    [Fact]
    public async Task Validator_WhenIdHasMoreThan6Characters_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { Id = "6009001" };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("must be 6 characters or fewer");
    }

    [Fact]
    public async Task Validator_WhenIdHasLessThan6Characters_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { Id = "60090" };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("must be at least 6 characters");
    }
}