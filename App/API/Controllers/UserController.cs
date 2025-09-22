using Application.Features.CQRS.Dtos;
using Application.Features.CQRS.Users.Commands.CreateUser;
using Application.Features.CQRS.Users.Commands.DeleteUser;
using Application.Features.CQRS.Users.Commands.UpdateUser;
using Application.Features.CQRS.Users.Queries.GetAllUsers;
using Application.Features.CQRS.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var users = await _mediator.Send(new GetUserQuery());
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
    public async Task<ActionResult> Put(UpdateUserCommand user)
    {
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
