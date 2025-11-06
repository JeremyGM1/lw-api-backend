using Application.Features.Identity.Permissions.Commands.AssignPermissionToRole;
using Application.Features.Identity.Permissions.Commands.AssignPermissionToUser;
using Application.Features.Identity.Permissions.Queries.GetAllPermissions;
using Application.Features.Identity.Permissions.Queries.GetAllPermissionsByRole;
using Application.Features.Identity.Permissions.Queries.GetPermissionsByRole;
using Application.Features.Identity.Permissions.Queries.GetPermissionsByUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }

    [HttpGet("role/{roleName}")]
    public async Task<IActionResult> GetByRole(string roleName)
    {
        var result = await _mediator.Send(new GetPermissionsByRoleQuery { RoleName = roleName });
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
    {
        var result = await _mediator.Send(new GetPermissionsByUserQuery { UserId = userId });
        return Ok(result);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRolesWithPermissions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid user context.");

        var result = await _mediator.Send(new GetAllPermissionsByRoleQuery(userId));
        return Ok(result);
    }

    [HttpPost("assign/role")]
    public async Task<IActionResult> AssignToRole([FromBody] AssignPermissionsToRoleCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("assign/user")]
    public async Task<IActionResult> AssignToUser([FromBody] AssignPermissionsToUserCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}