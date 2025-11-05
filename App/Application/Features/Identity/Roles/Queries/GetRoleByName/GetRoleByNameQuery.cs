using Application.Features.Identity.Models.RoleService;
using MediatR;
namespace Application.Features.Identity.Roles.Queries.GetRoleByName
{
    public record GetRoleByNameQuery(string RoleName) : IRequest<RoleDto>;
}
