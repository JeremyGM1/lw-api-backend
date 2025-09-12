using Moq;
using Domain.Entities;
using Persistence.Interfaces;

namespace App.Core.Tests.Unit.Application.Mocks;

public class MockUserRepository
{
    public static Mock<IUserRepository> GetMockUsersRepository()
    {
        var userList = new List<User>
        {
            new User
            {
                Id = "a3f9c8e2-7b4d-4c9e-bf2a-9d1f3e6a1c4",
                FirstName = "John",
                LastName = "Doe",
                Email = "JoDoe@gmail.com",
                UserRoles = null
            },
            new User
            {
                Id = "d7e2a1b5-3c6f-4f8a-9e2d-5a9c7f4b2e3d",
                FirstName = "Marcus",
                LastName = "Chen",
                Email = "marcus.chen@example.com",
                UserRoles = null
            },
            new User
            {
                Id = "f1c4d9a7-8e3b-4a1f-9c2d-7b6e5f3a2d1e",
                FirstName = "Priya",
                LastName = "Desai",
                Email = "priya.desai@example.com",
                UserRoles = null
            }
        };

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