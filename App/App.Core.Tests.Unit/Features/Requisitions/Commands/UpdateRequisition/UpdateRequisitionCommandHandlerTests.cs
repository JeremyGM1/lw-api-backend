using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.MappingProfiles;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using App.Core.Tests.Unit.TestData.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IGenericLogService<RequisitionLog>> _mockLogger;
    private readonly UpdateRequisitionCommandHandler _handler;

    public UpdateRequisitionCommandHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IGenericLogService<RequisitionLog>>();
        _handler = new UpdateRequisitionCommandHandler(
            _mapper,
            _mockReqRepository.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task UpdateHandler_WhenRequestIsValid_ShouldUpdateRequisition()
    {
        var command = RequisitionTestData.UpdateRequisitionCommand();
        var existingRequisition = RequisitionTestData.CreateRequisition();
        var validationResult = new ValidationResult();

        const string userEmail = "TestUser@email.com";
        var ipAddress = It.IsAny<string>();
        var userAgent = It.IsAny<string>();

        _mockCommonHelpers
            .Setup(h => h.GetLoggingContext())
            .Returns((userEmail, ipAddress, userAgent));

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Requisition>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();

        _mockReqRepository
            .Verify(r => r.GetRequisitionById(command.ReqCode!),
            Times.Once);

        _mockReqRepository
            .Verify(r => r.UpdateAsync(It.IsAny<Requisition>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHandler_WhenIdDoesNotExist_ShouldReturnResultFailure()
    {
        var command = RequisitionTestData.UpdateRequisitionCommand();
        command.ReqCode = "600901";

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldContain("NotExisting");

        _mockReqRepository
            .Verify(r => r.CreateAsync(It.IsAny<Requisition>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateHandler_WhenUploadSuccessfully_ShouldPreserveAuditFields()
    {
        var command = RequisitionTestData.UpdateRequisitionCommand();
        var existingRequisition = RequisitionTestData.CreateRequisition();
        existingRequisition.CreatedAt = DateTime.UtcNow.AddDays(-7);
        existingRequisition.CreatedBy = "original.creator@email.com";
        const string UserEmail = "test@email.com";
        const string IpAddress = "192.168.1.1";
        const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

        _mockCommonHelpers
            .Setup(h => h.GetLoggingContext())
            .Returns((UserEmail, IpAddress, UserAgent));

        Requisition capturedRequisition = null!;

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(existingRequisition);

        _mockReqRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Requisition>()))
            .Callback<Requisition>(req => capturedRequisition = req)
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        capturedRequisition.CreatedAt.ShouldBe(existingRequisition.CreatedAt);
        capturedRequisition.CreatedBy.ShouldBe(existingRequisition.CreatedBy);
        capturedRequisition.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        capturedRequisition.UpdatedBy.ShouldBe("test@email.com");
    }
}