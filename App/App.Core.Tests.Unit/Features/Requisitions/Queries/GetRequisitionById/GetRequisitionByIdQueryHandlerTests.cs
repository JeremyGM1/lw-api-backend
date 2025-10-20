using Application.Exceptions;
using Application.Features.Requisitions.MappingProfiles;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Requisitions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

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
    public async Task Handler_WhenIdDoesNotExist_ShouldThrowNotFoundException()
    {
        var query = new GetRequisitionByIdQuery("600901");

        _mockReqRepository
            .Setup(r => r.GetRequisitionById(query.Id))
            .ReturnsAsync((Requisition)null!);

        var result = await Should.ThrowAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        
        result.ShouldBeOfType<NotFoundException>();
        result.Message.ShouldBe("does not exist");
    }
}