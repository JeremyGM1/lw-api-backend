using App.Core.Tests.Unit.Application.Mocks;
using Application.Features.Shared.Dtos.UserDtos;
using Application.Features.Shared.MappingProfiles;
using Application.Features.Users.Queries.GetAllUsers;
using AutoMapper;
using LoggingService.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;
using Shouldly;

namespace App.Core.Tests.Unit.Application.Features.User.Queries;
public class GetUserListsQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly Mock<IAppLogger<GetUsersQueryHandler>> _mockAppLogger;
    private readonly ILoggerFactory loggerFactory;

    public GetUserListsQueryHandlerTests()
    {
        _mockRepo = MockUserRepository.GetMockUsersRepository();
        loggerFactory = new LoggerFactory();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>(), loggerFactory);

        _mapper = mapperConfig.CreateMapper();

        _mockAppLogger = new Mock<IAppLogger<GetUsersQueryHandler>>();
    }

    [Fact]
    public async Task GetUserListTest()
    {
        const int numberOfUsersInList = 3;

        var handler = new GetUsersQueryHandler(_mapper, _mockRepo.Object, _mockAppLogger.Object);

        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        result.ShouldBeOfType<List<UserDto>>();
        result.Count.ShouldBe(numberOfUsersInList);
    }
}