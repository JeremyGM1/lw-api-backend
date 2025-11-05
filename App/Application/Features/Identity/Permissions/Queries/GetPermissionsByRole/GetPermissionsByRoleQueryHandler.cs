using Application.Features.Identity.Models.PermissionService;
using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetPermissionsByRole;
public class GetPermissionsByRoleQueryHandler : IRequestHandler<GetPermissionsByRoleQuery, RolePermissionsDto>
{
    private readonly IPermissionService _permissionService;

    public GetPermissionsByRoleQueryHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<RolePermissionsDto> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionService.GetPermissionsByRoleNameAsync(request.RoleName);
        return new RolePermissionsDto
        {
            RoleName = request.RoleName,
            Permissions = permissions.Select(p => new PermissionDto
            {
                Name = p.Name,
                Description = p.Description
            }).ToList()
        };
    }
}