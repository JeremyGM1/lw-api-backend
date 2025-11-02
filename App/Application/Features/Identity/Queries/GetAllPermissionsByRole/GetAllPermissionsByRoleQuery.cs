using Application.Features.Identity.Models;
using MediatR;

namespace Application.Features.Identity.Queries.GetAllPermissionsByRole
{
    public record GetAllPermissionsByRoleQuery : IRequest<IReadOnlyList<RolePermissionsDto>>;
}
