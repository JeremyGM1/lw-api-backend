using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDById;
public record GetJDByIdQuery(int Id) : IRequest<Result<GetJobDescriptionResponseDto>>;