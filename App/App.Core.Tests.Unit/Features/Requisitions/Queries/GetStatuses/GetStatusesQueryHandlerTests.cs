using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetStatuses;
using Application.Interfaces.Lookup;
using Domain.Entities.Requisitions;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetStatuses;

public class GetStatusesQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IReqStatusRepository> _mockReqStatusRepository;
    private readonly GetReqStatusesQueryHandler _handler;

    public GetStatusesQueryHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqStatusRepository = new Mock<IReqStatusRepository>();
        _handler = new GetReqStatusesQueryHandler(
            _mapper,
            _mockReqStatusRepository.Object
        );
    }

    //[Fact]
    //public async Task Handler_WhenStatusExists_ShouldReturnListOfAllStatuses()
    //{
    //    var query = new GetReqStatusesQuery();

    //    var statusList = new List<RequisitionStatus>
    //    {
    //        new() { Id = 1, Status = "Cancelled" },
    //        new() { Id = 2, Status = "Closed" },
    //        new() { Id = 3, Status = "Completed" },
    //        new() { Id = 4, Status = "Open" },
    //    };

    //    _mockReqStatusRepository
    //        .Setup(r => r.GetStatuses())
    //        .ReturnsAsync(statusList);

    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    result.ShouldNotBeNull();
    //    result.Count.ShouldBe(4);
    //    result[0].Id.ShouldBe(1);
    //    result[0].Status.ShouldBe("Cancelled");
    //    result[1].Id.ShouldBe(2);
    //    result[1].Status.ShouldBe("Closed");
    //    result[2].Id.ShouldBe(3);
    //    result[2].Status.ShouldBe("Completed");
    //    result[3].Id.ShouldBe(4);
    //    result[3].Status.ShouldBe("Open");

    //    _mockReqStatusRepository
    //        .Verify(r => r.GetStatuses(),
    //        Times.Once);
    //}

    [Fact]
    public async Task Handler_WhenNoStatusExist_ShouldReturnEmptyList()
    {
        var query = new GetReqStatusesQuery();

        _mockReqStatusRepository
            .Setup(r => r.GetStatuses())
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();

        _mockReqStatusRepository
            .Verify(r => r.GetStatuses(),
            Times.Once);
    }
}