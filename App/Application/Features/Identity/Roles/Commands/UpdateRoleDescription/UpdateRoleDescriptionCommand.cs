using MediatR;

namespace Application.Features.Identity.Roles.Commands.UpdateRoleDescription
{
    public record UpdateRoleDescriptionCommand(string RoleName, string Description) : IRequest<bool>;
}
