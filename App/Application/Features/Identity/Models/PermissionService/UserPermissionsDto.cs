namespace Application.Features.Identity.Models.PermissionService
{
    public class UserPermissionsDto
    {
        public string UserId { get; set; } = default!;
        public IReadOnlyList<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
