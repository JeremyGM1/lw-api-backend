using Application.Common.Helpers;
using Application.Exceptions;
using Application.Features.Identity.Models;
using FluentAssertions;
using Identity.Models;
using Identity.Services;
using Moq;
using Shouldly;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests;
public class RegisterUserTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<ICommonHelpers> _mockCommonHelpers;
    private readonly UserService _userService;

    public RegisterUserTests()
    {
        _mockUserManager = CreateMockUserManager();
        _mockCommonHelpers = new Mock<ICommonHelpers>();
        _userService = new UserService(
            _mockUserManager.Object,
            _mockCommonHelpers.Object
        );
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

    private static ApplicationUser CreateTestUser(string id = "test-user-ud")
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

    private static RegistrationRequest CreateRegistrationRequest()
    {
        return new RegistrationRequest
        {
            Email = "test@example.com",
            Password = "TestPassword123!",
            FirstName = "John",
            LastName = "Doe"
        };
    }

    [Fact]
    public async Task Register_ValidRequest_UserIsCreated()
    {
        var request = CreateRegistrationRequest();

        _mockUserManager
            .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Administrator"))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userService.Register(request);

        result.ShouldNotBe(null);
        result.UserId.ShouldNotBe(null);
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
        _mockUserManager.Verify(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Administrator"), Times.Once);
    }

    [Fact]
    public async Task Register_UserAlreadyExist_ThrowBadRequestException()
    {
        var request = CreateRegistrationRequest();
        var user = CreateTestUser();
        var identityError = new IdentityError { Description = "Username is already taken" };

        _mockUserManager
            .Setup(r => r.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _mockUserManager
            .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request));

        exception.Message.ShouldContain(identityError.Description);
    }

    [Theory]
    [InlineData("password")]
    [InlineData("Password")]
    [InlineData("Passw0rd")]
    [InlineData("P@ssword")]
    public async Task Register_PasswordTooWeak_ThrowBadRequestException(string password)
    {
        var request = CreateRegistrationRequest();
        request.Password = password;

        var identityError = new IdentityError { Description = "Password does not meet requirements" };

        _mockUserManager
            .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), password))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        var exception = await Should.ThrowAsync<BadRequestException>(()=> _userService.Register(request));

        exception.Message.ShouldContain(identityError.Description);
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), password), Times.Once);
    }

    [Fact]
    public async Task Register_FirstNameMissing_ThrowBadRequestException()
    {
        var user = CreateTestUser();
        var request = new RegistrationRequest
        {
            FirstName = null!,
            LastName = "Doe",
            Email = "test@email.com",
            Password = "P@ssw0rd",
        };

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request));

        exception.Message.ShouldContain("The FirstName field is required");
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
    }

    [Fact]
    public async Task Register_LastNameMissing_ThrowBadRequestException()
    {
        var request = new RegistrationRequest
        {
            FirstName = "John",
            LastName = null!,
            Email = "test@email.com",
            Password = "P@ssw0rd",
        };

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request));

        exception.Message.ShouldContain("The LastName field is required");
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
    }

    [Fact]
    public async Task Register_EmailMissing_ThrowBadRequestException()
    {
        var request = new RegistrationRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = null!,
            Password = "P@ssw0rd",
        };

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request));

        exception.Message.ShouldContain("The Email field is required");
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
    }

    [Fact]
    public async Task Register_PasswordMissing_ThrowBadRequestException()
    {
        var request = new RegistrationRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "test@email.com",
            Password = null!,
        };

        var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request));

        exception.Message.ShouldContain("The Password field is required");
        _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
    }
}