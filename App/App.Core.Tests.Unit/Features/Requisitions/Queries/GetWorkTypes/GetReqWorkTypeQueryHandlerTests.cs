using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetWorkTypes;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Lookup.Requisition;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetWorkTypes;

public class GetReqWorkTypeQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IReqWorkTypeRepository> _mockReqWorkTypeRepository;
    private readonly GetReqWorkTypeQueryHandler _handler;

    public GetReqWorkTypeQueryHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqWorkTypeRepository = new Mock<IReqWorkTypeRepository>();
        _handler = new GetReqWorkTypeQueryHandler(
            _mapper,
            _mockReqWorkTypeRepository.Object
        );
    }

    //[Fact]
    //public async Task Handler_WhenWorkTypeExists_ShouldReturnListOfAllWorkTypes()
    //{
    //    var query = new GetReqWorkTypeQuery();

    //    var workTypeList = new List<RequisitionWorkType>
    //    {
    //        new() { Id = 1, WorkType = "Contractor" },
    //        new() { Id = 2, WorkType = "Fulltime" },
    //        new() { Id = 3, WorkType = "Internship" },
    //        new() { Id = 4, WorkType = "PartTime" },
    //    };

    //    _mockReqWorkTypeRepository
    //        .Setup(r => r.GetWorkTypes())
    //        .ReturnsAsync(workTypeList);

    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    result.ShouldNotBeNull();
    //    result.Count.ShouldBe(4);
    //    result[0].Id.ShouldBe(1);
    //    result[0].WorkType.ShouldBe("Contractor");
    //    result[1].Id.ShouldBe(2);
    //    result[1].WorkType.ShouldBe("Fulltime");
    //    result[2].Id.ShouldBe(3);
    //    result[2].WorkType.ShouldBe("Internship");
    //    result[3].Id.ShouldBe(4);
    //    result[3].WorkType.ShouldBe("PartTime");

    //    _mockReqWorkTypeRepository
    //        .Verify(r => r.GetWorkTypes(),
    //        Times.Once);
    //}

    [Fact]
    public async Task Handler_WhenNoWorkTypeExist_ShouldReturnEmptyList()
    {
        var query = new GetReqWorkTypeQuery();

        _mockReqWorkTypeRepository
            .Setup(r => r.GetWorkTypes())
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();

        _mockReqWorkTypeRepository
            .Verify(r => r.GetWorkTypes(),
            Times.Once);
    }
}