using Application.Features.Identity.Models.PermissionService;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetPermissionsByUserQuery;
public class GetPermissionsByUserQuery : IRequest<UserPermissionsDto>
{
    public string UserId { get; set; } = default!;
}
