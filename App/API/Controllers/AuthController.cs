using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Application.Features.Identity.Models;
using Application.Interfaces.Identity;
using Application.Exceptions;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;

    public AuthController(IAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("LoginPolicy")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        return Ok(await _authenticationService.Login(request));
    }
}