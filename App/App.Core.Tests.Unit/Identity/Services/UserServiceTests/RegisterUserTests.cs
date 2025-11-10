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

        private static RegistrationRequest CreateRequest(
            string? first = "John", string? last = "Doe", string? email = "test@email.com", string? pass = "P@ssw0rd")
            => new()
            {
                FirstName = first!,
                LastName = last!,
                Email = email!,
                Password = pass!,
                Role = SystemRoles.RegularUser
            };

        private static ApplicationUser CreateTestUser(string id = "admin-id")
            => new()
            {
                Id = id,
                Email = "admin@email.com",
                UserName = "admin@email.com",
                FirstName = "Admin",
                LastName = "User"
            };

        private void SetupRequester(string requesterId, string requesterRole)
        {
            var requester = CreateTestUser(requesterId);

            _mockUserManager
                .Setup(r => r.FindByIdAsync(requesterId))
                .ReturnsAsync(requester);

            _mockUserManager
                .Setup(r => r.GetRolesAsync(requester))
                .ReturnsAsync(new List<string> { requesterRole });
        }

        [Fact]
        public async Task Register_ValidRequest_ShouldCreateUserSuccessfully()
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);

            var request = CreateRequest();
            request.Role = SystemRoles.Administrator;

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role)).ReturnsAsync(IdentityResult.Success);

            var result = await _userService.Register(request, "admin-id");

            result.ShouldNotBeNull(); 
            result.UserId.ShouldNotBeNullOrEmpty();

            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _mockUserManager.Verify(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role), Times.Once);
        }

        [Theory]
        [InlineData(null, "Doe", "john@email.com", "P@ssw0rd", "FirstName")]
        [InlineData("John", null, "john@email.com", "P@ssw0rd", "LastName")]
        [InlineData("John", "Doe", null, "P@ssw0rd", "Email")]
        [InlineData("John", "Doe", "john@email.com", null, "Password")]
        public async Task Register_MissingRequiredField_ShouldThrowBadRequest(string first, string last, string email, string password, string fieldName)
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);
            var request = CreateRequest(first, last, email, password);

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));
            ex.Message.ShouldContain($"The {fieldName} field is required");
        }

        [Fact]
        public async Task Register_RoleDoesNotExist_ShouldThrowBadRequest()
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);

            var request = CreateRequest();
            request.Role = "NonExistentRole";

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(false);

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));
            ex.Message.ShouldContain($"The role '{request.Role}' does not exist.");
        }

        [Fact]
        public async Task Register_RequesterCannotAssignHigherRole_ShouldThrowBadRequestException()
        {
            SetupRequester("user-id", SystemRoles.RegularUser);

            var request = CreateRequest();
            request.Role = SystemRoles.Administrator; 

            _mockRoleManager
                .Setup(r => r.RoleExistsAsync(request.Role))
                .ReturnsAsync(true);

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "user-id"));
            ex.Message.ShouldContain("You cannot assign the role");
        }

        [Fact]
        public async Task Register_RequesterCanAssignSameLevelRole_ShouldCreateUserSuccessfully()
        {
            SetupRequester("admin-id", SystemRoles.Administrator);

            var request = CreateRequest();
            request.Role = SystemRoles.Administrator; 

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role)).ReturnsAsync(IdentityResult.Success);

            var result = await _userService.Register(request, "admin-id");

            result.ShouldNotBeNull();
            result.UserId.ShouldNotBeNullOrEmpty();
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _mockUserManager.Verify(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role), Times.Once);
        }

        [Fact]
        public async Task Register_RequesterCanAssignLowerLevelRole_ShouldCreateUserSuccessfully()
        {
            SetupRequester("admin-id", SystemRoles.Administrator);

            var request = CreateRequest();
            request.Role = SystemRoles.RegularUser;

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role)).ReturnsAsync(IdentityResult.Success);

            var result = await _userService.Register(request, "admin-id");

            result.ShouldNotBeNull();
            result.UserId.ShouldNotBeNullOrEmpty();
            _mockUserManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _mockUserManager.Verify(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role), Times.Once);
        }

        [Fact]
        public async Task Register_CreateUserFails_ShouldThrowBadRequest()
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);

            var request = CreateRequest();
            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);

            var identityError = new IdentityError { Description = "Password does not meet requirements" };
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));
            ex.Message.ShouldContain("Password does not meet requirements");
        }

        [Fact]
        public async Task Register_AddToRoleFails_ShouldThrowBadRequest()
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);

            var request = CreateRequest();
            request.Role = SystemRoles.Administrator;

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ReturnsAsync(true);
            _mockUserManager.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);

            var roleError = new IdentityError { Description = "Failed to add role" };
            _mockUserManager.Setup(r => r.AddToRoleAsync(It.IsAny<ApplicationUser>(), request.Role))
                .ReturnsAsync(IdentityResult.Failed(roleError));

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));
            ex.Message.ShouldContain("Unable to assign role");
            ex.Message.ShouldContain("Failed to add role");
        }

        [Fact]
        public async Task Register_UnexpectedException_ShouldWrapInBadRequest()
        {
            SetupRequester("admin-id", SystemRoles.SuperAdministrator);

            var request = CreateRequest();
            request.Role = SystemRoles.Administrator;

            _mockRoleManager.Setup(r => r.RoleExistsAsync(request.Role)).ThrowsAsync(new InvalidOperationException("Unexpected DB error"));

            var ex = await Should.ThrowAsync<BadRequestException>(() => _userService.Register(request, "admin-id"));
            ex.Message.ShouldContain("Error System.InvalidOperationException");
        }
    }
}
