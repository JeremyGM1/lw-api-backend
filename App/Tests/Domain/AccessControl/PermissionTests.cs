using Domain.AccessControl;
using Xunit;
using Moq;

namespace Domain.Tests.AccessControl
{
	public class PermissionTests
	{
		[Fact]
		public void Constructor_WhenCreatedWithValidProperties_SetsPropertiesCorrectly()
		{
			var name = "ReadUsers";
			var description = "Allows reading user data";

			var permission = new Permission
			{
				Name = name,
				Description = description,
				CreatedBy = "system"
            };

			Assert.Equal(name, permission.Name);
			Assert.Equal(description, permission.Description);
			Assert.NotNull(permission.RolePermissions);
			Assert.Empty(permission.RolePermissions);
			Assert.True(permission.IsActive);
		}

		[Fact]
		public void RolePermissions_WhenAddingRolePermission_AddsSuccessfully()
		{
			var mockRolePermission = new Mock<RolePermission>();
			var permission = new Permission
			{
				Name = "Write",
				Description = "Write access",
                CreatedBy = "system"
            };

			permission.RolePermissions.Add(mockRolePermission.Object);

			Assert.Single(permission.RolePermissions);
			Assert.Contains(mockRolePermission.Object, permission.RolePermissions);
		}

		[Fact]
		public void RolePermissions_WhenAddingMultipleRolePermissions_AddsAllSuccessfully()
		{
			var mockRolePermission1 = new Mock<RolePermission>();
			var mockRolePermission2 = new Mock<RolePermission>();
			var permission = new Permission
			{
				Name = "View Analytics",
				Description = "Can view app analytics",
                CreatedBy = "system"
            };

			permission.RolePermissions.Add(mockRolePermission1.Object);
			permission.RolePermissions.Add(mockRolePermission2.Object);

			Assert.Equal(2, permission.RolePermissions.Count);
			Assert.Contains(mockRolePermission1.Object, permission.RolePermissions);
			Assert.Contains(mockRolePermission2.Object, permission.RolePermissions);
		}
	}
}