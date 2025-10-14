using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using Application.Common.Helpers;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Microsoft.AspNetCore.Http;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands;
public class CreateRequisitionCommandHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<IValidator<CreateRequisitionCommand>> _mockValidator;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly CreateRequisitionCommandHandler _handler;

    public CreateRequisitionCommandHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockValidator = new Mock<IValidator<CreateRequisitionCommand>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _handler = new CreateRequisitionCommandHandler(
            _mockMapper.Object,
            _mockReqRepository.Object,
            _mockValidator.Object,
            _mockCommonHelpers.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateRequisition_WhenDataIsValid()
    {
        var requisition = TestData.CreateRequisitionCommand();
        var mockRequisition = TestData.CreateRequisition(requisition);
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(requisition, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCommonHelpers
            .Setup(ch => ch.ValidateForeighKey(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _mockCommonHelpers
            .Setup(ch => ch.GetCurrentUserFromToken())
            .Returns("TestUser@email.com");

        _mockMapper
            .Setup(m => m.Map<Requisition>(requisition))
            .Returns(mockRequisition);

        _mockReqRepository
            .Setup(r => r.CreateAsync(mockRequisition))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(requisition, CancellationToken.None);

        result.ShouldBe(MediatR.Unit.Value);
        _mockReqRepository
            .Verify(r => r.CreateAsync(It.Is<Requisition>(req => req.Id == "600900")),
            Times.Once
        );
    }

    [Fact]
    public async Task Validator_ExistingId_ReturnsValidationError()
    {
        var command = TestData.CreateRequisitionCommand();
        var requisition = TestData.CreateRequisition(command);
        var validationResult = new ValidationResult();


    }
    //public Task ValidationThrowsException
    //public Task ValidateForeignKeys
    //public Task ValidateAuditFields
}