using Application.Features.Identity.Models.PermissionService;
using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetPermissionsByUserQuery;
public class GetPermissionsByUserQueryHandler : IRequestHandler<GetPermissionsByUserQuery, UserPermissionsDto>
{
    private readonly IPermissionService _permissionService;

    public GetPermissionsByUserQueryHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<UserPermissionsDto> Handle(GetPermissionsByUserQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionService.GetPermissionsByUserIdAsync(request.UserId);
        return new UserPermissionsDto
        {
            UserId = request.UserId,
            Permissions = permissions.Select(p => new PermissionDto
            {
                Name = p.Name,
                Description = p.Description
            }).ToList()
        };
    }
}
