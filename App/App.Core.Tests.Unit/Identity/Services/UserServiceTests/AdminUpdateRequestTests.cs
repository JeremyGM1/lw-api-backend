using Application.Exceptions;
using Application.Features.Identity.Models.UserService;
using Identity.Configurations;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests;

public class AdminUpdateRequestTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
    private readonly UserService _userService;

    public AdminUpdateRequestTests()
    {
        _mockUserManager = CreateMockUserManager();
        _mockRoleManager = CreateMockRoleManager();
        _userService = new UserService(_mockUserManager.Object, _mockRoleManager.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var mock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        mock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync([]);

        return mock;
    }

    private static Mock<RoleManager<ApplicationRole>> CreateMockRoleManager()
    {
        return new Mock<RoleManager<ApplicationRole>>(
            new Mock<IRoleStore<ApplicationRole>>().Object,
            Array.Empty<IRoleValidator<ApplicationRole>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<ApplicationRole>>>().Object
        );
    }

    private static ApplicationUser CreateUser(string id = "user-id")
    {
        return new ApplicationUser
        {
            Id = id,
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };
    }

    private static AdminUpdateRequest CreateRequest(string id = "user-id", string? role = null)
    {
        return new AdminUpdateRequest
        {
            Id = id,
            Email = "updated@example.com",
            FirstName = "Updated",
            LastName = "User",
            Role = role,
            NewPassword = "NewPassword123!"
        };
    }

    [Fact]
    public async Task AdminUpdateRequest_ValidRequest_ReturnsUpdateResponse()
    {
        var targetUser = CreateUser("user-id");
        var adminUser = CreateUser("admin-id");
        var request = CreateRequest();

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(targetUser);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.Administrator]);
        _mockUserManager.Setup(u => u.GetRolesAsync(targetUser)).ReturnsAsync([SystemRoles.RegularUser]);
        _mockUserManager.Setup(u => u.GeneratePasswordResetTokenAsync(targetUser)).ReturnsAsync("token");
        _mockUserManager.Setup(u => u.ResetPasswordAsync(targetUser, "token", request.NewPassword!))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.UpdateAsync(targetUser)).ReturnsAsync(IdentityResult.Success);
        _mockRoleManager.Setup(r => r.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var result = await _userService.AdminUpdateRequest(request, "admin-id");

        result.ShouldNotBeNull();
        result.UserId.ShouldBe("user-id");
        targetUser.Email.ShouldBe(request.Email);
        targetUser.FirstName.ShouldBe(request.FirstName);
        targetUser.LastName.ShouldBe(request.LastName);
    }

    [Fact]
    public async Task AdminUpdateRequest_UserNotFound_ShouldThrowNotFoundException()
    {
        var request = CreateRequest();

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync((ApplicationUser?)null);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.AdminUpdateRequest(request, "admin-id"));
        ex.Message.ShouldContain("not found");
    }

    [Fact]
    public async Task AdminUpdateRequest_RoleDoesNotExist_ShouldThrowValidationException()
    {
        var user = CreateUser();
        var adminUser = CreateUser("admin-id");
        var request = CreateRequest("user-id", "FakeRole");

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.Administrator]);
        _mockRoleManager.Setup(r => r.RoleExistsAsync("FakeRole")).ReturnsAsync(false);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.AdminUpdateRequest(request, "admin-id"));
        ex.Message.ShouldContain("does not exist");
    }

    [Fact]
    public async Task AdminUpdateRequest_AssignHigherRole_ShouldThrowUnauthorizedAccess()
    {
        var user = CreateUser();
        var adminUser = CreateUser("admin-id");
        var request = CreateRequest("user-id", SystemRoles.SuperAdministrator);

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.Administrator]);
        _mockRoleManager.Setup(r => r.RoleExistsAsync(SystemRoles.SuperAdministrator)).ReturnsAsync(true);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.AdminUpdateRequest(request, "admin-id"));
        ex.Message.ShouldContain("higher-level role");
    }

    [Fact]
    public async Task AdminUpdateRequest_RequesterCannotModifyHigherRole_ShouldThrowUnauthorized()
    {
        var user = CreateUser();
        var regularUser = CreateUser("regular-id");
        var request = CreateRequest("user-id");

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("regular-id")).ReturnsAsync(regularUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(regularUser)).ReturnsAsync([SystemRoles.RegularUser]);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync([SystemRoles.Administrator]);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.AdminUpdateRequest(request, "regular-id"));
        ex.Message.ShouldContain("cannot modify a user with role");
    }

    [Fact]
    public async Task AdminUpdateRequest_ResetPasswordFails_ShouldThrowValidationException()
    {
        var user = CreateUser();
        var adminUser = CreateUser("admin-id");
        var request = CreateRequest();

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.SuperAdministrator]);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync([SystemRoles.RegularUser]);
        _mockUserManager.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
        _mockUserManager.Setup(u => u.ResetPasswordAsync(user, "token", request.NewPassword!))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Reset failed." }));

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.AdminUpdateRequest(request, "admin-id"));
        ex.Message.ShouldContain("Password reset failed");
    }

    [Fact]
    public async Task AdminUpdateRequest_UserUpdatesSelf_Success()
    {
        var user = CreateUser("self-id");
        var request = CreateRequest("self-id");

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync([SystemRoles.RegularUser]);
        _mockUserManager.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
        _mockUserManager.Setup(u => u.ResetPasswordAsync(user, "token", request.NewPassword!))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var result = await _userService.AdminUpdateRequest(request, "self-id");

        result.ShouldNotBeNull();
        result.UserId.ShouldBe("self-id");
        user.Email.ShouldBe(request.Email);
        user.FirstName.ShouldBe(request.FirstName);
        user.LastName.ShouldBe(request.LastName);
    }
}
