using Application.Features.Identity.Models.PermissionService;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetPermissionsByRole;
public class GetPermissionsByRoleQuery : IRequest<RolePermissionsDto>
{
    public string RoleName { get; set; } = default!;
}
