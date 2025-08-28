using Domain.AccessControl;
using Xunit;
using Moq;

namespace Domain.Tests.AccessControl
{
	public class RoleTests
	{
		[Fact]
		public void Role_Should_Be_Created_With_Valid_Properties()
		{
			var name = "Admin";
			var description = "Administrator role with full permissions";
			var roleCategoryId = "cat-123";
			var role = new Role
			{
				Name = name,
				Description = description,
				RoleCategoryId = roleCategoryId,
                CreatedBy = "system"
            };

			Assert.Equal(name, role.Name);
			Assert.Equal(description, role.Description);
			Assert.Equal(roleCategoryId, role.RoleCategoryId);
			Assert.NotNull(role.UserRoles);
			Assert.Empty(role.UserRoles);
			Assert.NotNull(role.RolePermissions);
			Assert.Empty(role.RolePermissions);
			Assert.True(role.IsActive);
		}

		[Fact]
		public void Role_ShouldAllowAddingUserRoles()
		{
			var mockUserRole = new Mock<UserRole>();
			var role = new Role
			{
				Name = "Editor",
				Description = "Can edit content",
				RoleCategoryId = "cat-456",
                CreatedBy = "system"
            };
			role.UserRoles.Add(mockUserRole.Object);
			Assert.Single(role.UserRoles);
			Assert.Contains(mockUserRole.Object, role.UserRoles);
		}

		[Fact]
		public void Role_ShouldHoldMultipleUserRoles()
		{
			var mockUserRole1 = new Mock<UserRole>();
			var mockUserRole2 = new Mock<UserRole>();
			var role = new Role
			{
				Name = "Viewer",
				Description = "Can view content",
				RoleCategoryId = "cat-789",
                CreatedBy = "system"
            };
			role.UserRoles.Add(mockUserRole1.Object);
			role.UserRoles.Add(mockUserRole2.Object);
			Assert.Equal(2, role.UserRoles.Count);
		}

		[Fact]
		public void Role_ShouldAllowAddingRolePermissions()
		{
			var mockRolePermission = new Mock<RolePermission>();
			var role = new Role
			{
				Name = "Regular User",
				Description = "Can interact with the App",
				RoleCategoryId = "cat-101",
                CreatedBy = "system"
            };
			role.RolePermissions.Add(mockRolePermission.Object);
			Assert.Single(role.RolePermissions);
			Assert.Contains(mockRolePermission.Object, role.RolePermissions);
		}

		[Fact]
		public void Role_ShouldHoldMultipleRolePermissions()
		{
			var mockRolePermission1 = new Mock<RolePermission>();
			var mockRolePermission2 = new Mock<RolePermission>();
			var role = new Role
			{
				Name = "Manager",
				Description = "Manages teams",
				RoleCategoryId = "cat-202",
                CreatedBy = "system"
            };
			role.RolePermissions.Add(mockRolePermission1.Object);
			role.RolePermissions.Add(mockRolePermission2.Object);
			Assert.Equal(2, role.RolePermissions.Count);
		}

		[Fact]
		public void GetActivePermissions_ShouldReturnOnlyActivePermissions()
		{
			var activePermission = new Permission
			{
				Name = "ActivePerm",
				Description = "An active permission",
                CreatedBy = "system"
            };
			var inactivePermission = new Permission
			{
				Name = "InactivePerm",
				Description = "An inactive permission",
                IsActive = false,
                CreatedBy = "system"
			};
			var rolePermission1 = new RolePermission
			{
				RoleId = "role-001",
				PermissionId = "perm-001",
				Permission = activePermission,
                CreatedBy = "system"
            };
			var rolePermission2 = new RolePermission
			{
				RoleId = "role-001",
				PermissionId = "perm-002",
				Permission = inactivePermission,
                CreatedBy = "system"
            };
			var role = new Role
			{
				Name = "TestRole",
				Description = "Role for testing",
				RoleCategoryId = "cat-303",
                CreatedBy = "system"
            };
			role.RolePermissions.Add(rolePermission1);
			role.RolePermissions.Add(rolePermission2);
			var activePermissions = role.GetActivePermissions().ToList();
			Assert.Single(activePermissions);
			Assert.Contains(activePermission, activePermissions);
			Assert.DoesNotContain(inactivePermission, activePermissions);
		}

	}
}