using Application.Features.Identity.Models.PermissionService;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetAllPermissionsByRole
{
    public record GetAllPermissionsByRoleQuery : IRequest<IReadOnlyList<RolePermissionsDto>>;
}
