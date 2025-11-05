using MediatR;

namespace Application.Features.Identity.Permissions.Commands.AssignPermissionToRole;
public class AssignPermissionsToRoleCommand : IRequest<Unit>
{
    public string RoleName { get; set; } = default!;
    public IEnumerable<string> PermissionNames { get; set; } = new List<string>();
    public string UpdatedBy { get; set; } = default!;
}
