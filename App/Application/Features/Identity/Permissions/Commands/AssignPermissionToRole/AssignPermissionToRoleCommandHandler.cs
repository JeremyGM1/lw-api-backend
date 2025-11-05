using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Permissions.Commands.AssignPermissionToRole;
    public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, Unit>
    {
        private readonly IPermissionService _permissionService;

        public AssignPermissionsToRoleCommandHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<Unit> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            await _permissionService.AssignPermissionsToRoleAsync(request.RoleName, request.PermissionNames, request.UpdatedBy);
            return Unit.Value;
        }
    }

