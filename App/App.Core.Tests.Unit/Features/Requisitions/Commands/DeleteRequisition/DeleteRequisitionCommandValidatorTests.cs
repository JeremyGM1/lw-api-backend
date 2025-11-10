using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Interfaces.Features.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public DeleteRequisitionCommandValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    [Fact]
    public async Task DeleteValidator_WhenIdExist_ShouldDeleteRequisition()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600900" };

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.ReqCode))
            .ReturnsAsync(true);

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task DeleteValidator_WhenIdIsNull_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { ReqCode = null! };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("is required");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteValidator_WhenIdIsEmpty_ShouldReturnFailure(string propertyValue)
    {
        var command = new DeleteRequisitionCommand { ReqCode = propertyValue };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("must not be empty");
    }

    [Fact]
    public async Task DeleteValidator_WhenIdHasMoreThan6Characters_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "6009001" };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("must be 6 characters or fewer");
    }

    [Fact]
    public async Task DeleteValidator_WhenIdHasLessThan6Characters_ShouldReturnFailure()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "60090" };

        var validator = new DeleteRequisitionCommandValidator(_mockReqRepository.Object);
        var result = await validator.ValidateAsync(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain("must be at least 6 characters");
    }
}