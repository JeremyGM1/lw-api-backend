using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Interfaces;
using Application.Exceptions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public GetRequisitionByIdQueryValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    [Fact]
    public async Task Validator_WhenIdDoesntExist_ShouldThrowBadRequestException()
    {
        const string id = "600901";
        var query = new GetRequisitionByIdQuery(id);

        var validator = new GetRequisitionByIdQueryValidator(_mockReqRepository.Object);

        _mockReqRepository
            .Setup(r => r.RequisitionExists(id))
            .ReturnsAsync(false);

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain("does not exist");
    }

    [Theory]
    [InlineData(null, "Id is required")]
    [InlineData("", "'Id' must not be empty")]
    [InlineData("   ", "'Id' must not be empty")]
    public async Task Validator_WhenIdIsNullOrEmpty_ShouldReturnFailure(string? id, string expectedError)
    {
        var query = new GetRequisitionByIdQuery(id!);

        var validator = new GetRequisitionByIdQueryValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("60040", "must be at least 6 characters")]
    [InlineData("6004044", "must be 6 characters or fewer")]
    public async Task Validator_WhenIdLengthIsDifferentThan6_ShouldReturnFailure(string id, string expectedError)
    {
        var query = TestData.CreateRequisitionCommand();
        query.Id = id;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Id");
        result.Errors.First(e => e.PropertyName == "Id")
            .ErrorMessage.ShouldContain(expectedError);
    }
}