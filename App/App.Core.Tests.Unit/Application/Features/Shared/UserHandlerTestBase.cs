using App.Core.Tests.Unit.Application.Mocks;
using Application.Features.Shared.MappingProfiles;
using AutoMapper;
using LoggingService.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace App.Core.Tests.Unit.Application.Features.Shared;
public abstract class UserHandlerTestBase<THandler>
{
    protected readonly Mock<IUserRepository> _mockRepo;
    protected readonly IMapper _mapper;
    protected readonly Mock<IAppLogger<THandler>> _mockAppLogger;
    protected readonly ILoggerFactory loggerFactory;

    protected UserHandlerTestBase()
    {
        _mockRepo = MockUserRepository.GetMockUsersRepository();
        loggerFactory = new LoggerFactory();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>(), loggerFactory);

        _mapper = mapperConfig.CreateMapper();

        _mockAppLogger = new Mock<IAppLogger<THandler>>();
    }
}