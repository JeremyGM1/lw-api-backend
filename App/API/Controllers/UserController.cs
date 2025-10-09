using System.Security.Claims;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Identity.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = $"{SystemRoles.Administrator},{SystemRoles.SuperAdministrator}")]
    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _userService.Register(request, userId));
    }

    [HttpPut("Update")]
    public async Task<ActionResult<UpdateResponse>> Update(UpdateRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _userService.Update(request, userId));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.DeleteUserAsync(id, userId);
        return NoContent();
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var profile = await _userService.GetCurrentUserProfileAsync(userId);

        if (profile == null)
            return NotFound();

        return Ok(profile);
    }

    [Authorize(Roles = $"{SystemRoles.Administrator},{SystemRoles.SuperAdministrator}")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid user context.");

        var users = await _userService.GetAllUsersByRoleAsync(userId);

        return Ok(users);
    }
}
