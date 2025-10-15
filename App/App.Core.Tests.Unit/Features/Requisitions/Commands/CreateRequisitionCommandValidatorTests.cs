using Application.Common.Helpers;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Commands;

public class CreateRequisitionCommandValidatorTests
{
    //private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    //private readonly Mock<IValidator<CreateRequisitionCommand>> _mockValidator;
    //private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    //private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    //private readonly CreateRequisitionCommandHandler _handler;

    public CreateRequisitionCommandValidatorTests()
    {
        //_mockMapper = new Mock<IMapper>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        //_mockValidator = new Mock<IValidator<CreateRequisitionCommand>>();
        //_mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //_mockCommonHelpers = new Mock<ICommonHelpers>();
        //_handler = new CreateRequisitionCommandHandler(
        //    _mockMapper.Object,
        //    _mockReqRepository.Object,
        //    _mockValidator.Object,
        //    _mockCommonHelpers.Object
        //);
    }

    [Fact]
    public async Task Validator_ExistingId_ReturnsValidationError()
    {
        var command = TestData.CreateRequisitionCommand();

        _mockReqRepository
            .Setup(r => r.RequisitionExists(command.Id))
            .ReturnsAsync(true);

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(command);

        result.ShouldNotBeNull();
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldBe("Requisition with this Id already exist.");
    }

    [Fact]
    public async Task Validator_Validates_ReturnsSuccessfulValidation()
    {
        
    }
}