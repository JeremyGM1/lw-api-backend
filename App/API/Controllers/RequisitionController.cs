using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Common.Dtos.RequisitionDtos.Lookup;
using Application.Features.Common.Dtos.RequisitionDtos;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.Queries.GetHardware;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Features.Requisitions.Queries.GetRequisitions;
using Application.Features.Requisitions.Queries.GetStatuses;
using Application.Features.Requisitions.Queries.GetWorkTypes;

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

    [HttpPut("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(string Id, [FromBody] UpdateRequisitionCommand requisition)
    {
        requisition.Id = Id;
        await _mediator.Send(requisition);
        return NoContent();
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(string Id)
    {
        var command = new DeleteRequisitionCommand { Id = Id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("statuses")]
    public async Task<List<GetReqStatusesDto>> GetStatuses()
    {
        return await _mediator.Send(new GetReqStatusesQuery());
    }

    [HttpGet("workType")]
    public async Task<List<GetReqWorkTypeDto>> GetWorkType()
    {
        return await _mediator.Send(new GetReqWorkTypeQuery());
    }

    [HttpGet("hardware")]
    public async Task<List<GetReqHardwareDto>> GetHardware()
    {
        return await _mediator.Send(new GetReqHardwareQuery());
    }
}