using Application.Exceptions;
using Application.Features.Identity.Models;
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

public class UpdateUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private readonly UserService _userService;

    public UpdateUserTests()
    {
        _mockUserManager = CreateMockUserManager();
        _mockRoleManager = CreateMockRoleManager();
        _userService = new UserService(_mockUserManager.Object, _mockRoleManager.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        userManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string>());

        return userManagerMock;
    }

    private static Mock<RoleManager<IdentityRole>> CreateMockRoleManager()
    {
        return new Mock<RoleManager<IdentityRole>>(
            new Mock<IRoleStore<IdentityRole>>().Object,
            Array.Empty<IRoleValidator<IdentityRole>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object
        );
    }

    private static ApplicationUser CreateTestUser(string id = "test-user-id")
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

    private static UpdateRequest CreateUpdateRequest(string userId = "test-user-id")
    {
        return new UpdateRequest
        {
            Id = userId,
            Email = "updated@example.com",
            FirstName = "Jane",
            LastName = "Smith",
            NewPassword = "NewPassword123!",
        };
    }

    [Fact]
    public async Task Update_ValidRequest_ReturnsUpdateResponse()
    {
        var request = CreateUpdateRequest();
        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.Update(request, "admin-id");

        result.ShouldNotBe(null);
        result.UserId.ShouldBe(request.Id);
        user.Email.ShouldBe(request.Email);
        user.UserName.ShouldBe(request.Email);
        user.FirstName.ShouldBe(request.FirstName);
        user.LastName.ShouldBe(request.LastName);
    }

    [Fact]
    public async Task Update_UserNotFound_ReturnsNotFoundException()
    {
        var request = CreateUpdateRequest();
        var adminUser = CreateTestUser("admin-id");

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync((ApplicationUser?)null);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        var exception = await Should.ThrowAsync<NotFoundException>(() => _userService.Update(request, "admin-id"));
        exception.Message.ShouldContain(request.Id);
    }

    [Fact]
    public async Task Update_IncorrectCurrentPassword_ReturnsValidationException()
    {
        var request = CreateUpdateRequest();
        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");
        var identityError = new IdentityError { Description = "Password change failed. Incorrect password." };

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var exception = await Should.ThrowAsync<ValidationException>(() => _userService.Update(request, "admin-id"));
        exception.Message.ShouldContain("incorrect password");

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Never);
    }

    [Fact]
    public async Task Update_OnlyEmailProvided_OnlyUpdatesEmail()
    {
        var request = new UpdateRequest
        {
            Id = "test-user-id",
            Email = "newEmail@example.com"
        };

        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");
        var originalFirstName = user.FirstName;
        var originalLastName = user.LastName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request, "admin-id");

        user.Email.ShouldBe(request.Email);
        user.UserName.ShouldBe(request.Email);
        user.FirstName.ShouldBe(originalFirstName);
        user.LastName.ShouldBe(originalLastName);
        user.PasswordHash.ShouldBe(originalPassword);

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Update_OnlyFirstNameProvided_OnlyUpdatesFirstName()
    {
        var request = new UpdateRequest
        {
            Id = "test-user-id",
            FirstName = "New-FirstName"
        };

        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");
        var originalEmail = user.Email;
        var originalLastName = user.LastName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request, "admin-id");

        user.FirstName.ShouldBe(request.FirstName);
        user.Email.ShouldBe(originalEmail);
        user.UserName.ShouldBe(originalEmail);
        user.LastName.ShouldBe(originalLastName);
        user.PasswordHash.ShouldBe(originalPassword);

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Update_OnlyLastNameProvided_OnlyUpdatesLastName()
    {
        var request = new UpdateRequest
        {
            Id = "test-user-id",
            LastName = "New-LastName"
        };

        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");
        var originalEmail = user.Email;
        var originalFirstName = user.FirstName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request, "admin-id");

        user.LastName.ShouldBe(request.LastName);
        user.Email.ShouldBe(originalEmail);
        user.UserName.ShouldBe(originalEmail);
        user.FirstName.ShouldBe(originalFirstName);
        user.PasswordHash.ShouldBe(originalPassword);

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    }

    //[Fact]
    //public async Task Update_ProvidesValidCurrentPasswordAndNewPassword_UpdatesPassword()
    //{
    //    var request = CreateUpdateRequest();
    //    var user = CreateTestUser(request.Id);
    //    var adminUser = CreateTestUser("admin-id");

    //    _mockUserManager
    //        .Setup(u => u.FindByIdAsync(request.Id))
    //        .ReturnsAsync(user);

    //    _mockUserManager
    //        .Setup(u => u.FindByIdAsync("admin-id"))
    //        .ReturnsAsync(adminUser);

    //    _mockUserManager
    //        .Setup(u => u.GetRolesAsync(adminUser))
    //        .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

    //    _mockUserManager
    //        .Setup(u => u.GetRolesAsync(user))
    //        .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

    //    _mockUserManager
    //        .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
    //        .ReturnsAsync(IdentityResult.Success);

    //    _mockUserManager
    //        .Setup(u => u.UpdateAsync(user))
    //        .ReturnsAsync(IdentityResult.Success);

    //    var result = await _userService.Update(request, "admin-id");

    //    result.ShouldNotBeNull();
    //    result.UserId.ShouldBe(request.Id);

    //    _mockUserManager.Verify(u => u.ResetPasswordAsync(
    //        It.Is<ApplicationUser>(u => u.Id == user.Id),
    //        token,
    //        request.NewPassword!),
    //        Times.Once
    //    );

    //    _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    //}

    [Fact]
    public async Task Update_UserTriesToUpdateOtherUserWithoutPermission_ThrowsUnauthorizedAccessException()
    {
        var request = CreateUpdateRequest();
        var user = CreateTestUser(request.Id);
        var regularUser = CreateTestUser("regular-user-id");

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("regular-user-id"))
            .ReturnsAsync(regularUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(regularUser))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _userService.Update(request, "regular-user-id"));

        exception.Message.ShouldContain("not allowed to update other users");
    }

    [Fact]
    public async Task Update_WithRoleChangeByNonSuperAdmin_ThrowsValidationException()
    {
        var request = new UpdateRequest
        {
            Id = "test-user-id",
            Role = SystemRoles.SuperAdministrator
        };

        var user = CreateTestUser(request.Id);
        var adminUser = CreateTestUser("admin-id");

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("admin-id"))
            .ReturnsAsync(adminUser);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(adminUser))
            .ReturnsAsync(new List<string> { SystemRoles.Administrator });

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        var exception = await Should.ThrowAsync<ValidationException>(
            () => _userService.Update(request, "admin-id"));

        exception.Message.ShouldContain("cannot assign the Super Administrator role");
    }

    [Fact]
    public async Task Update_UserUpdatesSelf_Successful()
    {
        var request = new UpdateRequest
        {
            Id = "self-user-id",
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName"
        };

        var user = CreateTestUser(request.Id);

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.FindByIdAsync("self-user-id"))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { SystemRoles.RegularUser });

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.Update(request, "self-user-id");

        result.ShouldNotBeNull();
        result.UserId.ShouldBe(request.Id);
        user.FirstName.ShouldBe(request.FirstName);
        user.LastName.ShouldBe(request.LastName);
    }
}