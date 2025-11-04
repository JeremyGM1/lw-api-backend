using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shouldly;
using Application.Exceptions;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.Features.Common;
using Application.Common.Result;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IRequisitionLogService> _mockLogger;
    private readonly CreateRequisitionCommandHandler _handler;

    public CreateRequisitionCommandHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IRequisitionLogService>();
        _handler = new CreateRequisitionCommandHandler(
            _mockMapper.Object,
            _mockReqRepository.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateHandler_WhenRequestIsValid_ShouldRegisterRequisition()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);
        var validationResult = new ValidationResult();

        _mockCommonHelpers
            .Setup(ch => ch.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _mockCommonHelpers
            .Setup(ch => ch.GetCurrentUserFromToken())
            .Returns("TestUser@email.com");

        _mockMapper
            .Setup(m => m.Map<Requisition>(command))
            .Returns(mockRequisition);

        _mockReqRepository
            .Setup(r => r.CreateAsync(mockRequisition))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldBe(Result.Success());
        _mockReqRepository
            .Verify(r => r.CreateAsync(It.Is<Requisition>(req => req.ReqCode == "600900")),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateHandler_WhenAuditFieldsAreSetCorrectly_ShouldSucceed()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);
        var validationResult = new ValidationResult();
        Requisition capturedRequisition = null!;

        const string userEmail = "TestUser@email.com";
        var ipAddress = It.IsAny<string>();
        var userAgent = It.IsAny<string>();

        _mockCommonHelpers
            .Setup(ch => ch.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _mockCommonHelpers
            .Setup(ch => ch.GetLoggingContext())
            .Returns((userEmail, ipAddress, userAgent));

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
    public async Task CreateHandler_WhenValidationFails_ShouldThrowBadRequestException()
    {
        var command = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(command);
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Invalid requisition", "Error message")
        ]);

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
            .Verify(r => r.CreateAsync(It.IsAny<Requisition>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateHandler_WhenForeignKeyIsInvalid_ShouldThrowForeignKeyNotFoundException()
    {
        var command = TestData.CreateRequisitionCommand();

        _mockCommonHelpers
            .Setup(h => h.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new ForeignKeyNotFoundException("Invalid foreign key"));

        var exception = await Should.ThrowAsync<ForeignKeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<ForeignKeyNotFoundException>();
        exception.Message.ShouldBe("Invalid foreign key");

        _mockReqRepository
            .Verify(r => r.CreateAsync(It.IsAny<Requisition>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateHandler_WhenUserIsNotAuthenticated_ShouldThrowsUnauthorizedAccessException()
    {
        var command = TestData.CreateRequisitionCommand();

        _mockCommonHelpers
            .Setup(h => h.GetLoggingContext())
            .Throws(new UnauthorizedAccessException("User not authenticated"));

        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<UnauthorizedAccessException>();
        exception.Message.ShouldBe("User not authenticated");

        _mockReqRepository
            .Verify(r => r.CreateAsync(It.IsAny<Requisition>()),
            Times.Never);
    }
}