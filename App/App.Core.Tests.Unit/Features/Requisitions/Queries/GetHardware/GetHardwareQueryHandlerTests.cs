using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetHardware;
using Application.Interfaces;
using Application.Interfaces.Lookup;
using AutoMapper;
using Domain.Entities.Requisitions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.Requisitions.Queries.GetHardware;
public class GetHardwareQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IReqHardwareRepository> _mockReqHardwareRepository;
    private readonly GetReqHardwareQueryHandler _handler;

    public GetHardwareQueryHandlerTests()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<RequisitionProfile>(), loggerFactory);
        _mapper = config.CreateMapper();

        _mockReqHardwareRepository = new Mock<IReqHardwareRepository>();
        _handler = new GetReqHardwareQueryHandler(
            _mapper,
            _mockReqHardwareRepository.Object
        );
    }

    [Fact]
    public async Task Handler_WhenHardwareExists_ShouldReturnListOfHardware()
    {
        var query = new GetReqHardwareQuery();

        var hardwareList = new List<RequisitionHardware>
        {
            new() { Id = 1, Hardware = "Dell" },
            new() { Id = 2, Hardware = "Mac" },
        };

        _mockReqHardwareRepository
            .Setup(r => r.GetHardware())
            .ReturnsAsync(hardwareList);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result[0].Id.ShouldBe(1);
        result[0].Hardware.ShouldBe("Dell");
        result[1].Id.ShouldBe(2);
        result[1].Hardware.ShouldBe("Mac");

        _mockReqHardwareRepository
            .Verify(r => r.GetHardware(),
            Times.Once);
    }

    [Fact]
    public async Task Handler_WhenNoHardwareExist_ShouldReturnEmptyList()
    {
        var query = new GetReqHardwareQuery();

        _mockReqHardwareRepository
            .Setup(r => r.GetHardware())
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();

        _mockReqHardwareRepository
            .Verify(r => r.GetHardware(),
            Times.Once);
    }
}