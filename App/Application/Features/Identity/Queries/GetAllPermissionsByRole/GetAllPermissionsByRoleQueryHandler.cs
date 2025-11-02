using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using MediatR;

namespace Application.Features.Identity.Queries.GetAllPermissionsByRole
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
