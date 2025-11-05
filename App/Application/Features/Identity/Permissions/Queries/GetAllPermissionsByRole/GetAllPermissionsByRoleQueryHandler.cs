using Application.Features.Identity.Models.PermissionService;
using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetAllPermissionsByRole
{
    public class GetAllPermissionsByRoleQueryHandler
    : IRequestHandler<GetAllPermissionsByRoleQuery, IReadOnlyList<RolePermissionsDto>>
    {
        private readonly IPermissionService _permissionService;

        public GetAllPermissionsByRoleQueryHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<IReadOnlyList<RolePermissionsDto>> Handle(GetAllPermissionsByRoleQuery request, CancellationToken cancellationToken)
        {
            return await _permissionService.GetAllRolesWithPermissionsAsync();
        }
    }
}
