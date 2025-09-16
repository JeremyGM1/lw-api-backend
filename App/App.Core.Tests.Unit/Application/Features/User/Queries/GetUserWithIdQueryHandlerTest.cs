using App.Core.Tests.Unit.Application.Mocks;
using Application.Features.Shared.Dtos.UserDtos;
using Application.Features.Shared.MappingProfiles;
using Application.Features.Users.Queries.GetUserById;
using AutoMapper;
using LoggingService.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;
using Shouldly;

namespace App.Core.Tests.Unit.Application.Features.User.Queries;
public class GetUserWithIdQueryHandlerTest
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly Mock<IAppLogger<GetUserByIdQueryHandler>> _mockAppLogger;
    private readonly ILoggerFactory loggerFactory;

    public GetUserWithIdQueryHandlerTest()
    {
        _mockRepo = MockUserRepository.GetMockUsersRepository();
        loggerFactory = new LoggerFactory();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>(), loggerFactory);

        _mapper = mapperConfig.CreateMapper();

        _mockAppLogger = new Mock<IAppLogger<GetUserByIdQueryHandler>>();
    }

    [Fact]
    public async Task GetUserWithIdTest()
    {
        var user = new Domain.Entities.Users.User
        {
            Id = "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@company.com",
            UserRoles = null
        };

        var handler = new GetUserByIdQueryHandler(_mapper, _mockRepo.Object);

        var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

        result.ShouldBeOfType<UserDto>();
        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);
    }
}