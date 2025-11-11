using Microsoft.AspNetCore.Mvc;
using API.Models.Errors;
using MediatR;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Commands.DeleteRequisition;
using Application.Features.Requisitions.Commands.UpdateRequisition;
using Application.Features.Requisitions.Dtos.Lookup;
using Application.Features.Requisitions.Dtos;
using Application.Features.Requisitions.Queries.GetCurrencies;
using Application.Features.Requisitions.Queries.GetHardware;
using Application.Features.Requisitions.Queries.GetInterval;
using Application.Features.Requisitions.Queries.GetLocation;
using Application.Features.Requisitions.Queries.GetLocationTypes;
using Application.Features.Requisitions.Queries.GetRequisitionById;
using Application.Features.Requisitions.Queries.GetRequisitions;
using Application.Features.Requisitions.Queries.GetStatuses;
using Application.Features.Requisitions.Queries.GetWorkTypes;
using Application.Features.Requisitions.Queries.GetReguisitionLogs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequisitionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetRequisitionDto>>> Get()
    {
        return await _mediator.Send(new GetRequisitionsQuery());
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetRequisitionDto>> GetById(string Id)
    {
        var result = await _mediator.Send(new GetRequisitionByIdQuery(Id));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CreateRequisitionDto>>> Post(CreateRequisitionCommand requisition)
    {
        var result = await _mediator.Send(requisition);
        if (result.IsSuccess)
        {
            var response = ApiResponse<CreateRequisitionDto>.FromResult(result);
            return Created(requisition.ReqCode, response);
        }

        var errorResponse = ApiResponse<CreateRequisitionDto>.FromResult(result);

        if (result.Error.Code.EndsWith("Existing"))
            return Conflict(errorResponse);

        return BadRequest(errorResponse);
    }

    [HttpPut("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(string Id, [FromBody] UpdateRequisitionCommand requisition)
    {
        requisition.ReqCode = Id;
        var result = await _mediator.Send(requisition);

        if (result.IsSuccess)
            return NoContent();

        var errorResponse = ApiResponse<UpdateRequisitionDto>.FromResult(result);

        if (result.Error.Code.EndsWith("NotExisting"))
            return NotFound(errorResponse);

        return BadRequest(errorResponse);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteRequisitionDto>> Delete(string Id)
    {
        var command = new DeleteRequisitionCommand { ReqCode = Id };
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return NoContent();

        var errorResponse = ApiResponse<DeleteRequisitionDto>.FromResult(result);

        if (result.Error.Code.EndsWith("NotExisting"))
            return NotFound(errorResponse);

        return BadRequest(errorResponse);
    }

    [HttpGet("ReqCode")]
    public async Task<ActionResult<GetRequisitionLogsDto>> GetReqLogsById(string ReqCode)
    {
        var result = await _mediator.Send(new GetRequisitionLogsByIdQuery(ReqCode));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("statuses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqStatusesDto>>> GetStatuses()
    {
        return await _mediator.Send(new GetReqStatusesQuery());
    }

    [HttpGet("workType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqWorkTypeDto>>> GetWorkType()
    {
        return await _mediator.Send(new GetReqWorkTypeQuery());
    }

    [HttpGet("hardware")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqHardwareDto>>> GetHardware()
    {
        return await _mediator.Send(new GetReqHardwareQuery());
    }

    [HttpGet("locationType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqLocationTypeDto>>> GetLocationTypes()
    {
        return await _mediator.Send(new GetReqLocationTypeQuery());
    }

    [HttpGet("location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqLocationDto>>> GetLocations()
    {
        return await _mediator.Send(new GetReqLocationQuery());
    }

    [HttpGet("compensationCurrency")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqCurrencyDto>>> GetCurrencies()
    {
        return await _mediator.Send(new GetReqCurrencyQuery());
    }

    [HttpGet("compensationInterval")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<GetReqIntervalDto>>> GetIntervals()
    {
        return await _mediator.Send(new GetReqIntervalQuery());
    }
}