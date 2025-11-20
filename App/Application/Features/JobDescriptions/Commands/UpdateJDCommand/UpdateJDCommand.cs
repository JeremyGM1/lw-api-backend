using MediatR;
using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;

namespace Application.Features.JobDescriptions.Commands.UpdateJDCommand;

public class UpdateJDCommand : IRequest<Result<UpdateJobDescriptionResponseDto>>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? PostingUrl { get; set; }
    public string? KeyResponsabilities { get; set; }
    public string? InternalNotes { get; set; }
}