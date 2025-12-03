using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Interfaces.Features.Requisitions;
using Domain.Entities.Requisitions;
using App.Core.Tests.Unit.TestData.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly GetRequisitionByIdQueryHandler _handler;

    public GetRequisitionByIdQueryHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqRepository = new Mock<IRequisitionRepository>();
        _handler = new GetRequisitionByIdQueryHandler(
            _mapper,
            _mockReqRepository.Object
        );
    }

    [Fact]
    public async Task GetRequisitionHandler_WhenIdExists_ShouldReturnRequisition()
    {
        var query = new GetRequisitionByIdQuery("600900");
        var requisition = RequisitionTestData.CreateRequisition();

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(query.ReqCode))
            .ReturnsAsync(requisition);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Data!.ReqCode.ShouldBe("600900");
    }

    [Fact]
    public async Task GetRequisitionHandler_WhenIdDoesntExist_ShouldThrowNotFoundException()
    {
        const string ReqCode = "600901";
        var query = new GetRequisitionByIdQuery(ReqCode);

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(ReqCode))
            .ReturnsAsync((Requisition)null!);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldContain("Requisitions.NotExisting");
    }
}