using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using MediatR;

namespace Application.Features.Identity.Queries.GetAllPermissions;
public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IReadOnlyList<PermissionDto>>
{
    private readonly IPermissionService _permissionService;

    public GetAllPermissionsQueryHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<IReadOnlyList<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();
        return permissions.Select(p => new PermissionDto
        {
            Name = p.Name,
            Description = p.Description
        }).ToList();
    }
}
