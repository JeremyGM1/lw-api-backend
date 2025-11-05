
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

namespace App.Core.Tests.Unit.Identity.Services.UserServiceTests
{
    public class RegisterUserTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
        private readonly UserService _userService;

        public RegisterUserTests()
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
            return new Mock<RoleManager<ApplicationRole>>(
                new Mock<IRoleStore<ApplicationRole>>().Object,
                Array.Empty<IRoleValidator<ApplicationRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object
            );
        }
        private static ApplicationUser CreateTestUser(string id = "test-user-id") => new()
        {
            Id = id,
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            EmailConfirmed = true
        };

        private static RegistrationRequest CreateRegistrationRequest() => new()
        {
            Email = "test@example.com",
            Password = "TestPassword123!",
            FirstName = "John",
            LastName = "Doe"
        };

        private void SetupAdminContext()
        {
            var adminUser = CreateTestUser("admin-id");

            _mockUserManager
                .Setup(r => r.FindByIdAsync("admin-id"))
                .ReturnsAsync(adminUser);

            _mockUserManager
                .Setup(r => r.GetRolesAsync(adminUser))
                .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

            _mockRoleManager
                .Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task Register_ValidRequest_UserIsCreated()
        {
            SetupAdminContext();

            var request = CreateRegistrationRequest();
            request.Role = SystemRoles.Administrator;

            _mockUserManager
                .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), SystemRoles.Administrator))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.Register(request, "admin-id");

            result.ShouldNotBeNull();
            result.UserId.ShouldNotBeNull();

            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _mockUserManager.Verify(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), SystemRoles.Administrator), Times.Once);
        }

        [Theory]
        [InlineData("password")]
        [InlineData("Password")]
        [InlineData("Passw0rd")]
        [InlineData("P@ssword")]
        public async Task Register_PasswordTooWeak_ThrowBadRequestException(string password)
        {
            SetupAdminContext();

            var request = CreateRegistrationRequest();
            request.Password = password;

            var identityError = new IdentityError { Description = "Password does not meet requirements" };

            _mockUserManager
                .Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain(identityError.Description);
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), password), Times.Once);
        }

        [Fact]
        public async Task Register_FirstNameMissing_ThrowBadRequestException()
        {
            SetupAdminContext();

            var request = new RegistrationRequest
            {
                FirstName = null!,
                LastName = "Doe",
                Email = "test@email.com",
                Password = "P@ssw0rd"
            };

            var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("The FirstName field is required");
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
        }

        [Fact]
        public async Task Register_LastNameMissing_ThrowBadRequestException()
        {
            SetupAdminContext();

            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = null!,
                Email = "test@email.com",
                Password = "P@ssw0rd"
            };

            var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("The LastName field is required");
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
        }

        [Fact]
        public async Task Register_EmailMissing_ThrowBadRequestException()
        {
            SetupAdminContext();

            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = null!,
                Password = "P@ssw0rd"
            };

            var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("The Email field is required");
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Never);
        }

        [Fact]
        public async Task Register_PasswordMissing_ThrowBadRequestException()
        {
            SetupAdminContext();

            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@email.com",
                Password = null!
            };

            var exception = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("The Password field is required");
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public async Task Register_RoleDoesNotExist_ThrowsBadRequestException()
        {
            var request = CreateRegistrationRequest();
            request.Role = "NonExistentRole";

            var adminUser = CreateTestUser("admin-id");

            _mockUserManager
                .Setup(r => r.FindByIdAsync("admin-id"))
                .ReturnsAsync(adminUser);

            _mockUserManager
                .Setup(r => r.GetRolesAsync(adminUser))
                .ReturnsAsync(new List<string> { SystemRoles.SuperAdministrator });

            _mockRoleManager
                .Setup(r => r.RoleExistsAsync("NonExistentRole"))
                .ReturnsAsync(false);

            var exception = await Should.ThrowAsync<BadRequestException>(
                () => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("The role 'NonExistentRole' does not exist.");
            _mockRoleManager.Verify(r => r.RoleExistsAsync("NonExistentRole"), Times.Once);
        }
        [Fact]
        public async Task Register_NonSuperAdminCreatesSuperAdmin_ThrowsUnauthorizedAccessException()
        {
            var request = CreateRegistrationRequest();
            request.Role = SystemRoles.SuperAdministrator;

            var normalAdmin = CreateTestUser("admin-id");

            _mockUserManager
                .Setup(r => r.FindByIdAsync("admin-id"))
                .ReturnsAsync(normalAdmin);

            _mockUserManager
                .Setup(r => r.GetRolesAsync(normalAdmin))
                .ReturnsAsync(new List<string> { SystemRoles.Administrator });

            _mockRoleManager
                .Setup(r => r.RoleExistsAsync(SystemRoles.SuperAdministrator))
                .ReturnsAsync(true);

            var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
                () => _userService.Register(request, "admin-id"));

            exception.Message.ShouldContain("Only Super Admins can create other Super Admins.");
            _mockRoleManager.Verify(r => r.RoleExistsAsync(SystemRoles.SuperAdministrator), Times.Once);
        }
    }
}
