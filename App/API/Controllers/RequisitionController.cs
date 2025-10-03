using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Shared.Dtos.RequisitionDtos;
using Application.Features.Requisitions.Queries;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequisitionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<GetRequisitionDto>>> Get()
    {
        return await _mediator.Send(new GetRequisitionsQuery());
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateRequisitionCommand requisition)
    {
        var response = await _mediator.Send(requisition);
        return CreatedAtAction(nameof(Get), new { id = response }, response);
    }

}