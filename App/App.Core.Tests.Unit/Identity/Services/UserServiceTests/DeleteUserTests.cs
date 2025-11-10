using Application.Exceptions;
using Identity.Configurations;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests;

public class DeleteUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
    private readonly UserService _userService;

    public DeleteUserTests()
    {
        _mockUserManager = CreateMockUserManager();
        _mockRoleManager = CreateMockRoleManager();
        _userService = new UserService(_mockUserManager.Object, _mockRoleManager.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        return new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );
    }

    private static Mock<RoleManager<ApplicationRole>> CreateMockRoleManager()
    {
        var store = new Mock<IRoleStore<ApplicationRole>>();
        return new Mock<RoleManager<ApplicationRole>>(
            store.Object,
            Array.Empty<IRoleValidator<ApplicationRole>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<ApplicationRole>>>().Object
        );
    }

    private static ApplicationUser CreateTestUser(string id) => new()
    {
        Id = id,
        Email = $"{id}@example.com",
        UserName = $"{id}@example.com",
        FirstName = "Test",
        LastName = "User"
    };

    private void SetupRequester(string id, string role)
    {
        var requester = CreateTestUser(id);
        _mockUserManager.Setup(u => u.FindByIdAsync(id)).ReturnsAsync(requester);
        _mockUserManager.Setup(u => u.GetRolesAsync(requester)).ReturnsAsync(new List<string> { role });
    }

    private void SetupTarget(string id, string role)
    {
        var target = CreateTestUser(id);
        _mockUserManager.Setup(u => u.FindByIdAsync(id)).ReturnsAsync(target);
        _mockUserManager.Setup(u => u.GetRolesAsync(target)).ReturnsAsync(new List<string> { role });
    }

    [Fact]
    public async Task Delete_SuperAdmin_DeletesAnyUser_ShouldReturnTrue()
    {
        SetupRequester("super-admin", SystemRoles.SuperAdministrator);
        SetupTarget("target-user", SystemRoles.RegularUser);

        _mockUserManager.Setup(u => u.DeleteAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.DeleteUserAsync("target-user", "super-admin");

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_SelfDeletion_ShouldReturnTrue()
    {
        SetupRequester("user-id", SystemRoles.RegularUser);
        SetupTarget("user-id", SystemRoles.RegularUser);

        _mockUserManager.Setup(u => u.DeleteAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.DeleteUserAsync("user-id", "user-id");

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_InvalidUserId_ShouldThrowBadRequestException()
    {
        _mockUserManager.Setup(u => u.FindByIdAsync("invalid-id")).ReturnsAsync((ApplicationUser?)null);
        SetupRequester("super-admin", SystemRoles.SuperAdministrator);

        var ex = await Should.ThrowAsync<BadRequestException>(() =>
            _userService.DeleteUserAsync("invalid-id", "super-admin"));

        ex.Message.ShouldContain("Error deleting user");
    }

    [Fact]
    public async Task Delete_AdminCannotDeleteSuperAdmin_ShouldThrowBadRequestException()
    {
        SetupRequester("admin", SystemRoles.Administrator);
        SetupTarget("super-admin", SystemRoles.SuperAdministrator);

        var ex = await Should.ThrowAsync<BadRequestException>(() =>
            _userService.DeleteUserAsync("super-admin", "admin"));

        ex.Message.ShouldContain("does not have permission");
    }

    [Fact]
    public async Task Delete_UnauthorizedDeletion_ShouldThrowBadRequestException()
    {
        SetupRequester("admin", SystemRoles.Administrator);
        SetupTarget("super-admin", SystemRoles.SuperAdministrator);

        var ex = await Should.ThrowAsync<BadRequestException>(() =>
            _userService.DeleteUserAsync("super-admin", "admin"));

        ex.Message.ShouldContain("does not have permission");
    }

    [Fact]
    public async Task Delete_DeleteFails_ShouldThrowBadRequestException()
    {
        SetupRequester("super-admin", SystemRoles.SuperAdministrator);
        SetupTarget("target", SystemRoles.RegularUser);

        _mockUserManager.Setup(u => u.DeleteAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Cannot delete user" }));

        var ex = await Should.ThrowAsync<BadRequestException>(() =>
            _userService.DeleteUserAsync("target", "super-admin"));

        ex.Message.ShouldContain("Cannot delete user");
    }
}
