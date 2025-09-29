using Application.Exceptions;
using Application.Features.Identity.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;
using Identity.Models;
using Identity.Services;

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests;

public class UpdateUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly UserService _userService;

    public UpdateUserTests()
    {
        _mockUserManager = CreateMockUserManager();
        _userService = new UserService(_mockUserManager.Object);
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var UserManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        return UserManagerMock;
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

    private static UpdateRequest CreateUpdateRequest(string userId = "test-user-id")
    {
        return new UpdateRequest
        {
            Id = userId,
            Email = "updated@example.com",
            FirstName = "Jane",
            LastName = "Smith",
            CurrentPassword = "TestPassword123@",
            NewPassword = "NewPassword123!",
        };
    }

    [Fact]
    public async Task Update_ValidRequest_ReturnsUpdateResponse()
    {
        var request = CreateUpdateRequest();
        var user = CreateTestUser(request.Id);

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.Update(request);

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

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync((ApplicationUser?)null);

        var exception = await Should.ThrowAsync<NotFoundException>(() => _userService.Update(request));
        exception.Message.ShouldContain(request.Id);
    }

    [Fact]
    public async Task Update_IncorrectCurrentPassword_ReturnsValidationException()
    {
        var request = CreateUpdateRequest();
        var user = CreateTestUser(request.Id);
        var identityError = new IdentityError { Description = "Current Password is incorrect" };

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var exception = await Should.ThrowAsync<ValidationException>(() => _userService.Update(request));
        exception.Message.ShouldContain("Current Password is incorrect");

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
        var originalFirstName = user.FirstName;
        var originalLastName = user.LastName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request);

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
        var origiralEmail = user.Email;
        var origiralLastName = user.LastName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request);

        user.FirstName.ShouldBe(request.FirstName);
        user.Email.ShouldBe(origiralEmail);
        user.UserName.ShouldBe(origiralEmail);
        user.LastName.ShouldBe(origiralLastName);
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
        var origiralEmail = user.Email;
        var originalFirstName = user.FirstName;
        var originalPassword = user.PasswordHash;

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.Update(request);

        user.LastName.ShouldBe(request.LastName);
        user.Email.ShouldBe(origiralEmail);
        user.UserName.ShouldBe(origiralEmail);
        user.FirstName.ShouldBe(originalFirstName);
        user.PasswordHash.ShouldBe(originalPassword);

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Update_ProvidesValidCurrentPasswordAndNewPassword_UpdatesPassword()
    {
        var request = CreateUpdateRequest();

        var user = CreateTestUser(request.Id);

        _mockUserManager
            .Setup(u => u.FindByIdAsync(request.Id))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(u => u.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(u => u.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.Update(request);

        result.ShouldNotBeNull();
        result.UserId.ShouldBe(request.Id);

        _mockUserManager.Verify(u => u.ChangePasswordAsync(
            It.Is<ApplicationUser>(u => u.Id == user.Id),
            request.CurrentPassword!,
            request.NewPassword!),
            Times.Once
        );

        _mockUserManager.Verify(u => u.UpdateAsync(user), Times.Once);
    }
}