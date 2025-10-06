using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Features.Requisitions.Queries.GetRequisitions;
using Application.Features.Shared.Dtos.RequisitionDtos;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequisitionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<List<GetRequisitionDto>> Get()
    {
        return await _mediator.Send(new GetRequisitionsQuery());
    }

    [HttpGet("{Id}")]
    public async Task<GetRequisitionDto> GetById(string Id)
    {
        return await _mediator.Send(new GetRequisitionByIdQuery(Id));
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateRequisitionCommand requisition)
    {
        var response = await _mediator.Send(requisition);
        return CreatedAtAction(nameof(Get), new { id = response }, response);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var command = new DeleteRequisitionCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

}