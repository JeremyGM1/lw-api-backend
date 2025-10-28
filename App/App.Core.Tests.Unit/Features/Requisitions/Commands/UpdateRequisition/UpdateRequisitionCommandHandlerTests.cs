using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.MappingProfiles;
using Application.Interfaces.LoggingService;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<IValidator<UpdateRequisitionCommand>> _mockValidator;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IRequisitionLogService> _mockLogger;
    private readonly UpdateRequisitionCommandHandler _handler;

    public UpdateRequisitionCommandHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockValidator = new Mock<IValidator<UpdateRequisitionCommand>>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IRequisitionLogService>();
        _handler = new UpdateRequisitionCommandHandler(
            _mapper,
            _mockReqRepository.Object,
            _mockValidator.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task Handler_WhenRequestIsValid_ShouldUpdateRequisition()
    {
        var command = TestData.UpdateRequisitionCommand();
        var existingRequisition = TestData.CreateRequisition();
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCommonHelpers
            .Setup(h => h.ThrowIfValidationFails(validationResult));

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(existingRequisition);

        _mockCommonHelpers
            .Setup(h => h.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _mockCommonHelpers
            .Setup(h => h.GetCurrentUserFromToken())
            .Returns("email@test.com");

        _mockReqRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Requisition>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldBe(MediatR.Unit.Value);

        _mockReqRepository
            .Verify(r => r.GetRequisitionById(command.ReqCode!),
            Times.Once);

        _mockReqRepository
            .Verify(r => r.UpdateAsync(It.Is<Requisition>(req =>
                req.ReqCode == command.ReqCode &&
                req.Name == command.Name &&
                req.UpdatedBy == "email@test.com"
            )),
            Times.Once);
    }

    [Fact]
    public async Task Handler_WhenValidationFailed_ShouldReturnBadRequestException()
    {
        var command = TestData.UpdateRequisitionCommand();
        var existingRequisition = TestData.CreateRequisition();
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Invalid requisition", "Error message")
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
        exception.Message.ShouldContain("Invalid requisition");

        _mockReqRepository
            .Verify(r => r.DeleteAsync(It.IsAny<Requisition>()),
            Times.Never);
    }

    [Fact]
    public async Task Handler_WhenIdDoesNotExist_ShouldThrowNotFoundException()
    {
        var command = TestData.UpdateRequisitionCommand();
        command.ReqCode = "600901";

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode))
            .ReturnsAsync((Requisition)null!);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<NotFoundException>();
        exception.Message.ShouldContain("was not found");

        _mockReqRepository
            .Verify(r => r.CreateAsync(It.IsAny<Requisition>()),
            Times.Never);
    }

    [Fact]
    public async Task Handler_WhenWrongForeignKeys_ShouldThrowForeignKeyNotFoundException()
    {
        var command = TestData.UpdateRequisitionCommand();
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(Mock.Of<Requisition>);

        _mockCommonHelpers
            .Setup(h => h.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new ForeignKeyNotFoundException("Invalid foreign key"));

        var exception = await Should.ThrowAsync<ForeignKeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldBeOfType<ForeignKeyNotFoundException>();
        exception.Message.ShouldContain("Invalid foreign key");
    }

    [Fact]
    public async Task Handler_WhenUserIsNotAuthenticated_ShouldThrowUnauthorizedAccessException()
    {
        var command = TestData.UpdateRequisitionCommand();
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(Mock.Of<Requisition>);

        _mockCommonHelpers
            .Setup(h => h.GetCurrentUserFromToken())
            .Throws(new UnauthorizedAccessException("User not authenticated"));

        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UnauthorizedAccessException>();
        exception.Message.ShouldContain("User not authenticated");
    }

    [Fact]
    public async Task Handler_WhenUploadSuccessfuly_ShouldPresertveAuditFields()
    {
        var command = TestData.UpdateRequisitionCommand();
        var existingRequisition = TestData.CreateRequisition();
        existingRequisition.CreatedAt = DateTime.UtcNow.AddDays(-7);
        existingRequisition.CreatedBy = "original.creator@email.com";

        var validationResult = new ValidationResult();
        Requisition capturedRequisition = null!;

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(command.ReqCode!))
            .ReturnsAsync(existingRequisition);

        _mockCommonHelpers
            .Setup(h => h.ValidateForeignKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _mockCommonHelpers
            .Setup(h => h.GetCurrentUserFromToken())
            .Returns("test@email.com");

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