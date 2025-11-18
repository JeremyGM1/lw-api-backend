using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Commands.CreateJobDescription;

public class CreateJDCommand : IRequest<Result<CreateJobDescriptionResponseDto>>
{
    public required string RequisitionCode { get; set; }
    public required string Title { get; set; }
    public required string PostingUrl { get; set; }
    public required string KeyResponsabilities { get; set; }
    public required string InternalNotes { get; set; }
}