using Application.Features.Identity.Models;
using MediatR;

namespace Application.Features.Identity.Queries.GetPermissionsByRole;
public class GetPermissionsByRoleQuery : IRequest<RolePermissionsDto>
{
    public string RoleName { get; set; } = default!;
}
