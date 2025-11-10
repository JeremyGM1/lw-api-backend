using Application.Common.Results;
using Application.Exceptions;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IRequisitionLogService> _mockLogger;
    private readonly DeleteRequisitionCommandHandler _handler;

    public DeleteRequisitionCommandTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IRequisitionLogService>();
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