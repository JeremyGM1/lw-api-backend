using Application.Features.Identity.Roles.Commands.UpdateRoleDescription;
using Application.Features.Identity.Roles.Queries.GetRoleByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetByName(string roleName)
        {
            var result = await _mediator.Send(new GetRoleByNameQuery(roleName));
            return Ok(result);
        }

        [HttpPut("{roleName}/description")]
        public async Task<IActionResult> UpdateDescription(string roleName, [FromBody] string description)
        {
            var result = await _mediator.Send(new UpdateRoleDescriptionCommand(roleName, description));
            return result ? Ok("Role description updated successfully.") : BadRequest("Failed to update role.");
        }
    }
}
