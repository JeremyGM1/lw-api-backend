using Application.Interfaces.Identity;
using MediatR;

namespace Application.Features.Identity.Commands.AssignPermissionToUser;
public class AssignPermissionsToUserCommandHandler : IRequestHandler<AssignPermissionsToUserCommand, Unit>
{
    private readonly IPermissionService _permissionService;

    public AssignPermissionsToUserCommandHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<Unit> Handle(AssignPermissionsToUserCommand request, CancellationToken cancellationToken)
    {
        await _permissionService.AssignPermissionsToUserAsync(request.UserId, request.PermissionNames, request.UpdatedBy);
        return Unit.Value;
    }
}

