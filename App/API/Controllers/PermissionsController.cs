using Application.Features.Identity.Commands.AssignPermissionToUser;
using Application.Features.Identity.Commands.AssingPermissionToRole;
using Application.Features.Identity.Queries.GetAllPermissions;
using Application.Features.Identity.Queries.GetPermissionsByRole;
using Application.Features.Identity.Queries.GetPermissionsByUserQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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