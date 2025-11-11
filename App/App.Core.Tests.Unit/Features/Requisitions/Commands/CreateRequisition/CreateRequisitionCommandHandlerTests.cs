using AutoMapper;
using FluentAssertions;
using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using Application.Common.Results.RequisitionErrors;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly CreateRequisitionCommandHandler _handler;

    public CreateRequisitionCommandHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<ILoggerService>();
        _handler = new CreateRequisitionCommandHandler(
            _mockMapper.Object,
            _mockReqRepository.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateHandler_WhenRequestIsValid_ShouldReturnResultSuccess()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        _mockMapper
            .Setup(m => m.Map<Requisition>(command))
            .Returns(mockRequisition);

        _mockReqRepository
            .Setup(r => r.CreateAsync(mockRequisition))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockReqRepository
            .Verify(r => r.CreateAsync(It.Is<Requisition>(req => req.ReqCode == "600900")),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateHandler_WhenReqCodeAlreadyExist_ShouldReturnResultFailure()
    {
        var command = TestData.CreateRequisitionCommand();
        var existingRequisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync(existingRequisition);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldContain("Requisitions.Existing");
    }

    [Fact]
    public async Task CreateHandler_WhenAuditFieldsAreSetCorrectly_ShouldSuccess()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);
        Requisition capturedRequisition = null!;

        const string userEmail = "TestUser@email.com";

        _mockCommonHelpers
            .Setup(ch => ch.GetLoggingContext())
            .Returns((userEmail, It.IsAny<string>(), It.IsAny<string>()));

        _mockMapper
            .Setup(m => m.Map<Requisition>(command))
            .Returns(mockRequisition);

        _mockReqRepository
            .Setup(r => r.CreateAsync(mockRequisition))
            .Callback<Requisition>((req) => capturedRequisition = req)
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        capturedRequisition.ShouldNotBeNull();
        capturedRequisition.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        capturedRequisition.CreatedBy.ShouldBe("TestUser@email.com");
    }

    [Fact]
    public async Task CreateHandler_WhenSuccessfulCreation_ShouldLogSuccess()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);

        const string userEmail = "TestUser@email.com";
        const string ipAddress = "198.163.1.10";
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

        _mockCommonHelpers
            .Setup(h => h.GetLoggingContext())
            .Returns((userEmail, ipAddress, userAgent));

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        _mockMapper
            .Setup(m => m.Map<Requisition>(command))
            .Returns(mockRequisition);

        _mockReqRepository
            .Setup(r => r.CreateAsync(mockRequisition))
            .Returns(Task.CompletedTask);

        _mockLogger
            .Setup(l => l.LogSuccessAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockLogger.Verify(v => v.LogSuccessAsync(
            command.ReqCode,
            "Create",
            userEmail,
            ipAddress,
            userAgent,
            "Success")
        ,Times.Once);
    }
}