using Application.Exceptions;
using FluentValidation;
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
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
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
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );
    }

    private static Mock<RoleManager<IdentityRole>> CreateMockRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        return new Mock<RoleManager<IdentityRole>>(
            store.Object,
            new IRoleValidator<IdentityRole>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object
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

    [Fact]
    public async Task Delete_SuperAdmin_DeletesAnyUser()
    {
        var superAdmin = CreateTestUser("super-admin");
        var target = CreateTestUser("target-user");

        // Mocked both users and their roles
        _mockUserManager.Setup(u => u.FindByIdAsync("super-admin")).ReturnsAsync(superAdmin);
        _mockUserManager.Setup(u => u.FindByIdAsync("target-user")).ReturnsAsync(target);

        _mockUserManager.Setup(u => u.GetRolesAsync(superAdmin))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });
        _mockUserManager.Setup(u => u.GetRolesAsync(target))
            .ReturnsAsync(new List<string> { "User" });

        _mockUserManager.Setup(u => u.DeleteAsync(target)).ReturnsAsync(IdentityResult.Success);

        var result = await _userService.DeleteUserAsync("target-user", "super-admin");

        result.ShouldBeTrue();
        _mockUserManager.Verify(u => u.DeleteAsync(target), Times.Once);
    }

    [Fact]
    public async Task Delete_AdminCannotDeleteSuperAdmin_ThrowsValidationException()
    {
        var admin = CreateTestUser("admin");
        var superAdmin = CreateTestUser("super-admin");

        _mockUserManager.Setup(u => u.FindByIdAsync("admin")).ReturnsAsync(admin);
        _mockUserManager.Setup(u => u.FindByIdAsync("super-admin")).ReturnsAsync(superAdmin);

        _mockUserManager.Setup(u => u.GetRolesAsync(admin))
            .ReturnsAsync(new List<string> { SystemRoles.Administrator });
        _mockUserManager.Setup(u => u.GetRolesAsync(superAdmin))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        await Should.ThrowAsync<ValidationException>(() =>
            _userService.DeleteUserAsync("super-admin", "admin"));
    }

    [Fact]
    public async Task Delete_UserWithoutPermission_ThrowsUnauthorizedAccessException()
    {
        var requester = CreateTestUser("normal-user");
        var target = CreateTestUser("target-user");

        _mockUserManager.Setup(u => u.FindByIdAsync("normal-user")).ReturnsAsync(requester);
        _mockUserManager.Setup(u => u.FindByIdAsync("target-user")).ReturnsAsync(target);

        _mockUserManager.Setup(u => u.GetRolesAsync(requester)).ReturnsAsync(new List<string> { "User" });
        _mockUserManager.Setup(u => u.GetRolesAsync(target)).ReturnsAsync(new List<string> { "User" });

        await Should.ThrowAsync<UnauthorizedAccessException>(() =>
            _userService.DeleteUserAsync("target-user", "normal-user"));
    }

    [Fact]
    public async Task Delete_SelfDeletion_Succeeds()
    {
        var user = CreateTestUser("user-id");

        _mockUserManager.Setup(u => u.FindByIdAsync("user-id")).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        _mockUserManager.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        var result = await _userService.DeleteUserAsync("user-id", "user-id");

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task Delete_InvalidUserId_ThrowsNotFoundException()
    {
        _mockUserManager.Setup(u => u.FindByIdAsync("invalid-id")).ReturnsAsync((ApplicationUser?)null);

        await Should.ThrowAsync<NotFoundException>(() =>
            _userService.DeleteUserAsync("invalid-id", "requester"));
    }

    [Fact]
    public async Task Delete_DeleteFails_ThrowsBadRequestException()
    {
        var requester = CreateTestUser("super-admin");
        var target = CreateTestUser("target");

        _mockUserManager.Setup(u => u.FindByIdAsync("super-admin")).ReturnsAsync(requester);
        _mockUserManager.Setup(u => u.FindByIdAsync("target")).ReturnsAsync(target);

        _mockUserManager.Setup(u => u.GetRolesAsync(requester))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });
        _mockUserManager.Setup(u => u.GetRolesAsync(target))
            .ReturnsAsync(new List<string> { "User" });

        _mockUserManager.Setup(u => u.DeleteAsync(target))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Cannot delete user" }));

        var ex = await Should.ThrowAsync<BadRequestException>(() =>
            _userService.DeleteUserAsync("target", "super-admin"));

        ex.Message.ShouldContain("Cannot delete user");
    }
}