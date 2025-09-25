using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Application.Exceptions;
using Identity.Models;
using Identity.Services;

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests;

public class DeleteUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly UserService _userService;

    public DeleteUserTests()
    {
        _mockUserManager = CreateMockUserManager();
        _userService = new UserService(_mockUserManager.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        return new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
    }

    private static ApplicationUser CreateTestUser(string id = "Tests-user-id")
    {
        return new ApplicationUser
        {
            Id = id,
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            EmailConfirmed = true,
        };
    }

    [Fact]
    public async Task Delete_ValidUserIdProvided_UserIsDeleted()
    {
        const string userId = "test-user-id";
        var user = CreateTestUser(userId);

        _mockUserManager
            .Setup(d => d.FindByIdAsync(user.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(d => d.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.DeleteUserAsync(user.Id);

        result.ShouldBeTrue();
        _mockUserManager.Verify(d => d.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task Delete_InvalidUserId_ThrowsNotFoundException()
    {
        const string userId = "invalid-user-id";

        _mockUserManager
            .Setup(d => d.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser?)null);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _userService.DeleteUserAsync(userId));
        _mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeleteFails_ThrowsBadRequestException()
    {
        const string userId = "test-user-id";
        var user = CreateTestUser(userId);
        var identityError = new IdentityError { Description = "Cannot delete user" };

        _mockUserManager
            .Setup(d => d.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(d => d.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.DeleteUserAsync(userId));
        exception.Message.ShouldContain("Cannot delete user");
    }
}