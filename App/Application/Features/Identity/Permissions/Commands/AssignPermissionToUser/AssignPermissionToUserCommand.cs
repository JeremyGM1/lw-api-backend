using MediatR;

namespace Application.Features.Identity.Permissions.Commands.AssignPermissionToUser;
public class AssignPermissionsToUserCommand : IRequest<Unit>
{
    public string UserId { get; set; } = default!;
    public IEnumerable<string> PermissionNames { get; set; } = new List<string>();
    public string UpdatedBy { get; set; } = default!;
}

