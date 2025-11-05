using Application.Features.Identity.Models.RoleService;
using Application.Interfaces.Features.Identity;
using MediatR;

namespace Application.Features.Identity.Roles.Queries.GetRoleByName
{
    public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, RoleDto>
    {
        private readonly IRoleService _roleService;

        public GetRoleByNameQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<RoleDto> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetRoleByNameAsync(request.RoleName);
        }
    }
}
