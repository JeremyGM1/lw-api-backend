using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<IValidator<GetRequisitionByIdQuery>> _mockValidator;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly GetRequisitionByIdQueryHandler _handler;

    public GetRequisitionByIdQueryHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockValidator = new Mock<IValidator<GetRequisitionByIdQuery>>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _handler = new GetRequisitionByIdQueryHandler(
            _mapper,
            _mockReqRepository.Object,
            _mockValidator.Object,
            _mockCommonHelpers.Object
        );
    }

    [Fact]
    public async Task Handler_WhenIdExists_ShouldReturnRequisition()
    {
        var query = new GetRequisitionByIdQuery("600900");
        var requisition = TestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(query.Id))
            .ReturnsAsync(requisition);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe("600900");
    }

    [Fact]
    public async Task Handler_WhenIdDoesntExist_ShouldThrowNotFoundException()
    {
        const string id = "600901";
        var query = new GetRequisitionByIdQuery(id);
        var validationResult = new ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(id))
            .ReturnsAsync((Requisition)null!);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

        exception.ShouldBeOfType<NotFoundException>();
        exception.Message.ShouldContain("was not found");
    }

    [Fact]
    public async Task Handler_WhenInvalidId_ShouldThrowBadRequestException()
    {
        var query = new GetRequisitionByIdQuery("");
        var validationResult = new ValidationResult(
        [
            new ValidationFailure("Id", "Validation failed")
        ]);

        _mockValidator
            .Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCommonHelpers
            .Setup(h => h.ThrowIfValidationFails(validationResult))
            .Throws(new BadRequestException("Validation Failed"));

        var result = await Should.ThrowAsync<BadRequestException>(() => _handler.Handle(query, CancellationToken.None));

        result.ShouldBeOfType<BadRequestException>();
        result.Message.ShouldContain("Validation failed");
    }
}