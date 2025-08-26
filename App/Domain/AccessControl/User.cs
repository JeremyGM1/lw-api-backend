namespace Domain.AccessControl
{
    public class User : BaseEntity
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public void AssignRole(Role role)
        {
            if (this.HasRole(role.Name))
            {
                return;
            }

            UserRoles.Add(new UserRole
            {
                UserId = this.Id,
                RoleId = role.Id,
                IsActive = true
            });
        }

        public void RemoveRole(string roleId)
        {
            var userRole = UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole != null)
                UserRoles.Remove(userRole);
        }

        public bool HasRole(string roleName)
        {
            return UserRoles.Any(ur => ur.Role.Name == roleName && ur.IsActive);
        }

        public IEnumerable<string> GetPermissions()
        {
            return UserRoles
                .Where(ur => ur.IsActive)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => rp.IsActive && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Name)
                .Distinct();
        }
    }
}