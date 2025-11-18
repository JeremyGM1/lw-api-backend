using FluentValidation.Results;
using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IGenericLogService<RequisitionLog>> _mockLogger;
    private readonly DeleteRequisitionCommandHandler _handler;

    public DeleteRequisitionCommandTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IGenericLogService<RequisitionLog>>();
        _handler = new DeleteRequisitionCommandHandler(
            _mockReqRepository.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
            );
    }

    [Fact]
    public async Task DeleteHandler_WhenRequestIsValid_ShouldDeleteRequisition()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600900" };
        var existingRequisition = TestData.CreateRequisition();
        var validationResult = new ValidationResult();

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.DeleteAsync(existingRequisition))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();

        _mockReqRepository
            .Verify(r => r.GetRequisitionById(command.ReqCode),
            Times.Once);

        _mockReqRepository
            .Verify(r => r.DeleteAsync(existingRequisition),
            Times.Once);
    }

    [Fact]
    public async Task DeleteHandler_WhenIdDoesNotExist_ShouldReturnResultFailure()
    {
        var command = new DeleteRequisitionCommand { ReqCode = "600901" };

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Requisitions.NotExisting");
    }
}