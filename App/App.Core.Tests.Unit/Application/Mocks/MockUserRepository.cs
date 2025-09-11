using Moq;
using Domain.Entities;
using Persistence.Interfaces;
using App.Core.Tests.Unit.Application.Mocks.DataFactory;

namespace App.Core.Tests.Unit.Application.Mocks;

public class MockUserRepository
{
    public static Mock<IUserRepository> GetMockUsersRepository()
    {
        List<User> users = null;

        var userList = users ?? TestDataFactory.CreateTestUserList();

        var mockRepo = new Mock<IUserRepository>();

        mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(userList);

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => userList.FirstOrDefault(u => u.Id == id));

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<User>()))
            .Returns((User user) =>
            {
                userList.Add(user);
                return Task.CompletedTask;
            });

        return mockRepo;
    }
}