using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shouldly;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<IValidator<DeleteRequisitionCommand>> _mockValidator;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<ILoggerHelpers> _mockLoggerHelpers;
    private readonly DeleteRequisitionCommandHandler _handler;

    public DeleteRequisitionCommandTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockValidator = new Mock<IValidator<DeleteRequisitionCommand>>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLoggerHelpers = new Mock<ILoggerHelpers>();
        _handler = new DeleteRequisitionCommandHandler(
            _mockReqRepository.Object,
            _mockValidator.Object,
            _mockCommonHelpers.Object,
            _mockLoggerHelpers.Object
            );
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldDeleteRequisition()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600900" };
        var existingRequisition = TestData.CreateRequisition();
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCommonHelpers
            .Setup(h => h.ThrowIfValidationFails(validationResult));

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.DeleteAsync(existingRequisition))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldBe(MediatR.Unit.Value);
        _mockReqRepository
            .Verify(r => r.GetRequisitionById(command.ReqCode),
            Times.Once);

        _mockReqRepository
            .Verify(r => r.DeleteAsync(existingRequisition),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowBadRequestException()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600901" };

        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Id", "Invalid Id")
        ]);

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCommonHelpers
            .Setup(h => h.ThrowIfValidationFails(validationResult))
            .Callback<ValidationResult>(vr =>
            {
                if (vr.Errors.Count != 0)
                    throw new BadRequestException("Invalid requisition", vr);
            });

        var exception = await Should.ThrowAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<BadRequestException>();
        exception.Message.ShouldBe("Invalid requisition");

        _mockReqRepository
            .Verify(r => r.GetRequisitionById(It.IsAny<string>()), Times.Never);
        _mockReqRepository
            .Verify(r => r.DeleteAsync(It.IsAny<Requisition>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIdDoesNotExist_ShouldThrowNotFoundException()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600901" };

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<NotFoundException>();
        exception.Message.ShouldContain("not found");
    }
}