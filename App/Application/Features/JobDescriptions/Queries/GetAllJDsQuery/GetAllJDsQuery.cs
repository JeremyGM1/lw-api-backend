using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetAllJDsQuery;
public record GetAllJDsQuery : IRequest<Result<List<GetJobDescriptionResponseDto>>>;