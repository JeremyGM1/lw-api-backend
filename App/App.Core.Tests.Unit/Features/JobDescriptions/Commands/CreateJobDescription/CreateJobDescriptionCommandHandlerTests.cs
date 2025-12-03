using App.Core.Tests.Unit.TestData.JobDescriptions;
using Application.Common.Results.RequisitionErrors;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.JobDescriptions;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using AutoMapper;
using Domain.Entities.JobDescriptions;
using Domain.Entities.Requisitions;
using FluentAssertions;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.JobDescriptions.Commands.CreateJobDescription;

public class CreateJobDescriptionCommandHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IJobDescriptionRepository> _mockJdRepository;
    private readonly Mock<IRequisitionRepository> _mockReqRepository;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly Mock<IJobDescriptionLoggingService> _mockLogger;
    private readonly CreateJDCommandHandler _handler;

    public CreateJobDescriptionCommandHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockJdRepository = new Mock<IJobDescriptionRepository>();
        _mockReqRepository = new Mock<IRequisitionRepository>();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _mockLogger = new Mock<IJobDescriptionLoggingService>();
        _handler = new CreateJDCommandHandler(
            _mockMapper.Object,
            _mockJdRepository.Object,
            _mockReqRepository.Object,
            _mockCommonHelpers.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task CreateJDHandler_WhenRequestIsValid_ShouldReturnSuccess()
    {
        var command = JobDescriptionTestData.CreateJobDescriptionCommand();
        var mockJD = JobDescriptionTestData.JobDescriptionEntity(command);
        var responseDto = JobDescriptionTestData.CreateJDDto(command);
        var mockRequisition = TestData.Requisitions.RequisitionTestData.CreateRequisition();

        _mockReqRepository.Setup(r => r.GetRequisitionById(command.RequisitionCode))
            .ReturnsAsync(mockRequisition);

        _mockMapper.Setup(m => m.Map<JobDescription>(command))
            .Returns(mockJD);

        _mockJdRepository.Setup(jd => jd.CreateAsync(mockJD))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CreateJobDescriptionResponseDto>(mockJD))
            .Returns(responseDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        _mockJdRepository
            .Verify(jd => jd.CreateAsync(It.Is<JobDescription>(jd => jd.ReqCode == "600900")),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateJDHandler_ShouldSetCreateAtUtcNow()
    {
        var command = JobDescriptionTestData.CreateJobDescriptionCommand();
        var mockJD = JobDescriptionTestData.JobDescriptionEntity(command);
        var responseDto = JobDescriptionTestData.CreateJDDto(command);
        var mockRequisition = TestData.Requisitions.RequisitionTestData.CreateRequisition();

        _mockReqRepository.Setup(r => r.GetRequisitionById(command.RequisitionCode))
            .ReturnsAsync(mockRequisition);

        _mockMapper.Setup(m => m.Map<JobDescription>(command))
            .Returns(mockJD);

        _mockJdRepository.Setup(jd => jd.CreateAsync(mockJD))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CreateJobDescriptionResponseDto>(mockJD))
            .Returns(responseDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateJDHandler_ShouldSetCreatedByEmailToUserEmail()
    {
        var command = JobDescriptionTestData.CreateJobDescriptionCommand();
        var mockJD = JobDescriptionTestData.JobDescriptionEntity(command);
        var responseDto = JobDescriptionTestData.CreateJDDto(command);
        var mockRequisition = TestData.Requisitions.RequisitionTestData.CreateRequisition();

        var userEmail = "TestUser@email.com";

        _mockCommonHelpers.Setup(c => c.GetLoggingContext())
            .Returns((userEmail, null!, null!));

        _mockReqRepository.Setup(r => r.GetRequisitionById(command.RequisitionCode))
            .ReturnsAsync(mockRequisition);

        _mockMapper.Setup(m => m.Map<JobDescription>(command))
            .Returns(mockJD);

        _mockJdRepository.Setup(jd => jd.CreateAsync(mockJD))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CreateJobDescriptionResponseDto>(mockJD))
            .Returns(responseDto);

        var result = await _handler.Handle(command, CancellationToken.None);
        
        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        _mockJdRepository
            .Verify(jd => jd.CreateAsync(It.Is<JobDescription>(jd => jd.CreatedBy == userEmail)),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateJDHandler_ShouldSetReqCodeFromRequisition()
    {
        var command = JobDescriptionTestData.CreateJobDescriptionCommand();
        var mockJD = JobDescriptionTestData.JobDescriptionEntity(command);
        var responseDto = JobDescriptionTestData.CreateJDDto(command);
        var mockRequisition = TestData.Requisitions.RequisitionTestData.CreateRequisition();

        _mockReqRepository.Setup(r => r.GetRequisitionById(command.RequisitionCode))
            .ReturnsAsync(mockRequisition);

        _mockMapper.Setup(m => m.Map<JobDescription>(command))
            .Returns(mockJD);

        _mockJdRepository.Setup(jd => jd.CreateAsync(mockJD))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CreateJobDescriptionResponseDto>(mockJD))
            .Returns(responseDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        _mockJdRepository
            .Verify(jd => jd.CreateAsync(It.Is<JobDescription>(jd => jd.ReqCode == mockRequisition.ReqCode)),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateJDHandler_WhenRequisitionNotFound_ShouldReturnFailure()
    {
        var command = JobDescriptionTestData.CreateJobDescriptionCommand();
        var mockRequisition = null as Requisition;

        _mockReqRepository.Setup(r => r.GetRequisitionById(command.RequisitionCode))
            .ReturnsAsync((Requisition)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Description.ShouldBe("A requisition with this code '600900' does not exists");
        _mockJdRepository.Verify(jd => jd.CreateAsync(It.IsAny<JobDescription>()),
            Times.Never);
    }
}