using AutoMapper;
using Moq;
using Shouldly;
using Application.Features.Requisitions.Dtos;
using Application.Features.Requisitions.Queries.GetRequisitions;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetRequisitions;

public class GetRequisitionsQueryHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<IAppLogger<GetRequisitionsQuery>> _mockLogger;
    private readonly GetRequisitionsQueryHandler _handler;

    public GetRequisitionsQueryHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockLogger = new Mock<IAppLogger<GetRequisitionsQuery>>();
        _handler = new GetRequisitionsQueryHandler(
            _mockMapper.Object,
            _mockReqRepository.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task GetRequisitionsHandler_WhenListItsNotEmpty_ShouldReturnAllRequisitions()
    {
        var query = new GetRequisitionsQuery();

        var requisitionList = new List<Requisition>
        {
           TestData.CreateRequisition(),
           TestData.CreateRequisition()
        };

        var expectedDtos = new List<GetRequisitionDto>
        {
            new() {Id = "600900", Name = "Test 1"},
            new() {Id = "600901", Name = "Test 2"},
        };

        _mockReqRepository
            .Setup(r => r.GetRequisitions())
            .ReturnsAsync(requisitionList);

        _mockMapper
            .Setup(m => m.Map<List<GetRequisitionDto>>(requisitionList))
            .Returns(expectedDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldBe(expectedDtos);
        result.Count.ShouldBe(2);
        _mockReqRepository
            .Verify(r => r.GetRequisitions(),
            Times.Once);
    }

    [Fact]
    public async Task GetRequisitionsHandler_WhenListItsEmpty_ShouldReturnEmptyList()
    {
        var query = new GetRequisitionsQuery();

        var requisitionList = new List<Requisition>();

        var expectedDtos = new List<GetRequisitionDto>();

        _mockReqRepository
            .Setup(r => r.GetRequisitions())
            .ReturnsAsync(requisitionList);

        _mockMapper
            .Setup(m => m.Map<List<GetRequisitionDto>>(requisitionList))
            .Returns(expectedDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldBeEmpty();
        result.ShouldNotBeNull();
        _mockReqRepository
            .Verify(r => r.GetRequisitions(),
            Times.Once);
    }
}