using Application.Features.Identity.Models;
using MediatR;

namespace Application.Features.Identity.Queries.GetPermissionsByUserQuery;
public class GetPermissionsByUserQuery : IRequest<UserPermissionsDto>
{
    public string UserId { get; set; } = default!;
}
