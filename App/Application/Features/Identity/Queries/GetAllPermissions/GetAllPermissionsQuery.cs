using Application.Features.Identity.Models;
using MediatR;

namespace Application.Features.Identity.Queries.GetAllPermissions;
public record GetAllPermissionsQuery() : IRequest<IReadOnlyList<PermissionDto>>;
