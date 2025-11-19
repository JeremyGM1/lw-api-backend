using API.Models.Errors;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Features.JobDescriptions.Commands.DeleteJobDescription;
using Application.Features.JobDescriptions.Dtos.Request;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobDescriptionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{ReqCode}")]
    public async Task<ActionResult<GetJobDescriptionResponseDto>> GetByReqCode(string ReqCode)
    {
        var result = await _mediator.Send(new GetJDsByRequisitionQuery(ReqCode));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

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
            InternalNotes = dto.InternalNotes
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

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteJobDescriptionResponseDto>> Delete(int id)
    {
        var command = new DeleteJDCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NoContent();

        var errorResponse = ApiResponse<DeleteJobDescriptionDto>.FromResult(result);

        if (result.Error.Code.EndsWith("NotExisting"))
            return NotFound(errorResponse);

        return BadRequest(errorResponse);
    }
}