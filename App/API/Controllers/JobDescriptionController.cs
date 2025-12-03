using API.Models.Errors;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Features.JobDescriptions.Commands.DeleteJobDescription;
using Application.Features.JobDescriptions.Commands.UpdateJDCommand;
using Application.Features.JobDescriptions.Dtos.Request;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Features.JobDescriptions.Queries.GetAllJDsQuery;
using Application.Features.JobDescriptions.Queries.GetJDById;
using Application.Features.JobDescriptions.Queries.GetJDLogs;
using Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;
using Application.Features.Requisitions.Dtos;
using Application.Features.Requisitions.Queries.GetReguisitionLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobDescriptionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetJobDescriptionResponseDto>> GetAllJobDescriptions()
    {
        var result = await _mediator.Send(new GetAllJDsQuery());

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("requisition/{reqCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetJobDescriptionResponseDto>> GetJDByReqCode(string reqCode)
    {
        var result = await _mediator.Send(new GetJDsByRequisitionQuery(reqCode));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetJobDescriptionResponseDto>> GetJDById(int id)
    {
        var result = await _mediator.Send(new GetJDByIdQuery(id));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpPost("{requisitionCode}/jobDescriptions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CreateJobDescriptionResponseDto>>> Post(string requisitionCode, CreateJobDescriptionDto dto)
    {
        var command = new CreateJDCommand
        {
            RequisitionCode = requisitionCode,
            Title = dto.Title,
            PostingUrl = dto.PostingUrl,
            KeyResponsabilities = dto.KeyResponsabilities,
            InternalNotes = dto.InternalNotes,
            RequiredSkills = dto.RequiredSkills,
            DesiredSkills = dto.DesiredSkills,
            BonusSkills = dto.BonusSkills
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            var response = ApiResponse<CreateJobDescriptionResponseDto>.FromResult(result);
            return Created(result.Data?.Title, response);
        }

        var errorResponse = ApiResponse<CreateJobDescriptionResponseDto>.FromResult(result);

        if (result.Error.Code.EndsWith("Existing"))
            return Conflict(errorResponse);

        return BadRequest(errorResponse);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UpdateJobDescriptionResponseDto>>> Update(int id, UpdateJobDescriptionDto dto)
    {
        var command = new UpdateJDCommand
        {
            Id = id,
            ReqCode = dto.ReqCode,
            Title = dto.Title,
            PostingUrl = dto.PostingUrl,
            KeyResponsabilities = dto.KeyResponsabilities,
            InternalNotes = dto.InternalNotes,
            RequiredSkills = dto.RequiredSkills,
            DesiredSkills = dto.DesiredSkills,
            BonusSkills = dto.BonusSkills
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return NoContent();

        var errorResponse = ApiResponse<UpdateJobDescriptionResponseDto>.FromResult(result);

        return BadRequest(errorResponse);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteJobDescriptionResponseDto>> Delete(int id)
    {
        var command = new DeleteJDCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return NoContent();

        var errorResponse = ApiResponse<DeleteJobDescriptionDto>.FromResult(result);

        if (result.Error.Code.EndsWith("NotExisting"))
            return NotFound(errorResponse);

        return BadRequest(errorResponse);
    }

    [HttpGet("{id}/logs")]
    public async Task<ActionResult<GetJobDescriptionLogsResponseDto>> GetJobDescriptionLogsById(int id)
    {
        var result = await _mediator.Send(new GetJDLogsByIdQuery(id));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }
}