using API.Models.Errors;
using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Features.JobDescriptions.Dtos;
using Application.Features.JobDescriptions.Dtos.Response;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobDescriptionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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
}