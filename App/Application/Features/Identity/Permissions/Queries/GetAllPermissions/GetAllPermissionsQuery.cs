using Application.Features.Identity.Models.PermissionService;
using MediatR;

namespace Application.Features.Identity.Permissions.Queries.GetAllPermissions;
public record GetAllPermissionsQuery() : IRequest<IReadOnlyList<PermissionDto>>;
