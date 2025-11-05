using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Roles.Commands.UpdateRoleDescription
{
    public class UpdateRoleDescriptionCommandHandler : IRequestHandler<UpdateRoleDescriptionCommand, bool>
    {
        private readonly IRoleService _roleService;

        public UpdateRoleDescriptionCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<bool> Handle(UpdateRoleDescriptionCommand request, CancellationToken cancellationToken)
        {
            return await _roleService.UpdateRoleDescriptionAsync(request.RoleName, request.Description);
        }
    }
}
