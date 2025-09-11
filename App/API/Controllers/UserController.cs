using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Shared.Dtos.UserDtos;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET: api/<UserController>
    [HttpGet]
    public async Task<List<UserDto>> Get()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return users;
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<UserDto> Get(string id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return user;
    }

    // POST api/<UserController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(CreateUserCommand user)
    {
        var response = await _mediator.Send(user);
        return CreatedAtAction(nameof(Get), new { id = response });
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(string id, [FromBody] UpdateUserCommand user)
    {
        user.Id = id;
        await _mediator.Send(user);
        return NoContent();
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(string id)
    {
        var command = new DeleteUserCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}