using Moq;
using Shouldly;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Interfaces.Features.Requisitions;
using App.Core.Tests.Unit.TestData.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryValidatorTests
{
    private readonly Mock<IRequisitionRepository> _mockReqRepository;

    public GetRequisitionByIdQueryValidatorTests()
    {
        _mockReqRepository = new Mock<IRequisitionRepository>();
    }

    [Theory]
    [InlineData(null, "is required")]
    [InlineData("", "must not be empty")]
    [InlineData("   ", "must not be empty")]
    public async Task GetRequisitionByIdValidator_WhenIdIsNullOrEmpty_ShouldReturnFailure(string? reqCode, string expectedError)
    {
        var query = new GetRequisitionByIdQuery(reqCode!);

        var validator = new GetRequisitionByIdQueryValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain(expectedError);
    }

    [Theory]
    [InlineData("60040", "must be at least 6 characters")]
    [InlineData("6004044", "must be 6 characters or fewer")]
    public async Task GetRequisitionByIdValidator_WhenIdLengthIsDifferentThan6_ShouldReturnFailure(string reqCode, string expectedError)
    {
        var query = RequisitionTestData.CreateRequisitionCommand();
        query.ReqCode = reqCode;

        var validator = new CreateRequisitionCommandValidator(_mockReqRepository.Object);

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "ReqCode");
        result.Errors.First(e => e.PropertyName == "ReqCode")
            .ErrorMessage.ShouldContain(expectedError);
    }
}