namespace Domain.AccessControl
{
    public class Role : BaseEntity
    {

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string RoleCategoryId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        public IEnumerable<Permission> GetActivePermissions()
        {
            return RolePermissions
                .Where(rp => rp.Permission.IsActive)
                .Select(rp => rp.Permission);
        }
    }
}