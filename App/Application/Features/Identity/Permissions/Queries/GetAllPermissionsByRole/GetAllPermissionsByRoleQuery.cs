using Application.Features.Identity.Models.PermissionService;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetAllPermissionsByRole
{
    public record GetAllPermissionsByRoleQuery(string userId) : IRequest<IReadOnlyList<RolePermissionsDto>>;
}
