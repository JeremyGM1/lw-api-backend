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

public class UpdateUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
    private readonly UserService _userService;

    public UpdateUserTests()
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
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );

        mock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

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

    private static ApplicationUser CreateUser(string id, string email = "user@email.com") =>
        new() { Id = id, Email = email, UserName = email, FirstName = "John", LastName = "Doe" };

    private static UpdateRequest CreateRequest(string id, string? role = null) =>
        new() { Id = id, Email = "updated@email.com", FirstName = "Jane", LastName = "Smith", Role = role };

    [Fact]
    public async Task Update_UserNotFound_ShouldThrowBadRequest()
    {
        var request = CreateRequest("missing-user");
        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id))
                        .ReturnsAsync((ApplicationUser?)null);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Update(request, "admin-id"));
        ex.Message.ShouldContain("not found");
    }

    [Fact]
    public async Task Update_RoleDoesNotExist_ShouldThrowBadRequest()
    {
        var user = CreateUser("user-id");
        var adminUser = CreateUser("admin-id");
        var request = CreateRequest("user-id", "NonExistentRole");

        _mockUserManager.Setup(u => u.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.Administrator]);
        _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(false);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Update(request, "admin-id"));
        ex.Message.ShouldContain("does not exist");
    }

    [Fact]
    public async Task Update_RequesterAssignsHigherRole_ShouldThrowBadRequest()
    {
        var targetUser = CreateUser("target-id");
        var adminUser = CreateUser("admin-id");

        _mockUserManager.Setup(u => u.FindByIdAsync(targetUser.Id)).ReturnsAsync(targetUser);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(adminUser);
        _mockUserManager.Setup(u => u.GetRolesAsync(adminUser)).ReturnsAsync([SystemRoles.Administrator]);
        _mockUserManager.Setup(u => u.GetRolesAsync(targetUser)).ReturnsAsync([SystemRoles.RegularUser]);

        var request = CreateRequest("target-id", SystemRoles.SuperAdministrator);
        _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Update(request, "admin-id"));
        ex.Message.ShouldContain("higher-level role");
    }

    [Fact]
    public async Task Update_RequesterCannotEditHigherUser_ShouldThrowBadRequest()
    {
        var admin = CreateUser("admin-id");
        var superAdmin = CreateUser("target-id");

        _mockUserManager.Setup(u => u.FindByIdAsync("target-id")).ReturnsAsync(superAdmin);
        _mockUserManager.Setup(u => u.FindByIdAsync("admin-id")).ReturnsAsync(admin);

        _mockUserManager.Setup(u => u.GetRolesAsync(admin)).ReturnsAsync([SystemRoles.Administrator]);
        _mockUserManager.Setup(u => u.GetRolesAsync(superAdmin)).ReturnsAsync([SystemRoles.SuperAdministrator]);

        var request = CreateRequest("target-id");

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Update(request, "admin-id"));
        ex.Message.ShouldContain("cannot update a user with role");
    }

    [Fact]
    public async Task Update_ValidData_ShouldUpdateSuccessfully()
    {
        var user = CreateUser("target-id");
        var superAdmin = CreateUser("super-id");

        _mockUserManager.Setup(u => u.FindByIdAsync("target-id")).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByIdAsync("super-id")).ReturnsAsync(superAdmin);
        _mockUserManager.Setup(u => u.GetRolesAsync(superAdmin)).ReturnsAsync([SystemRoles.SuperAdministrator]);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync([SystemRoles.RegularUser]);
        _mockUserManager.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var request = CreateRequest("target-id");
        var result = await _userService.Update(request, "super-id");

        result.ShouldNotBeNull();
        result.UserId.ShouldBe("target-id");
        user.Email.ShouldBe(request.Email);
        user.FirstName.ShouldBe(request.FirstName);
        user.LastName.ShouldBe(request.LastName);
    }

    [Fact]
    public async Task Update_UpdateFails_ShouldThrowBadRequest()
    {
        var user = CreateUser("user-id");
        _mockUserManager.Setup(u => u.FindByIdAsync("user-id")).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.UpdateAsync(user))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB Error" }));

        var request = CreateRequest("user-id");

        var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Update(request, "user-id"));
        ex.Message.ShouldContain("User update failed");
    }
}
