using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDLogs;
public record GetJDLogsByIdQuery(int id) : IRequest<Result<List<GetJobDescriptionLogsResponseDto>>>;