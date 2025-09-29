using Application.Features.CQRS.Dtos;
using Application.Features.CQRS.Users.Commands.CreateUser;
using Application.Features.CQRS.Users.Commands.DeleteUser;
using Application.Features.CQRS.Users.Commands.UpdateUser;
using Application.Features.CQRS.Users.Queries.GetAllUsers;
using Application.Features.CQRS.Users.Queries.GetUserById;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        return Ok(await _userService.Register(request));
    }
    [HttpPut("Update")]
    public async Task<ActionResult<UpdateResponse>> Update(UpdateRequest request)
    {
        return Ok(await _userService.Update(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
    [HttpGet("me")]
    [Authorize]
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
}