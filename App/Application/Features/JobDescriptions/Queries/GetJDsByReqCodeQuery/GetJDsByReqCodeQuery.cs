using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDsByReqCodeQuery;
public record GetJDsByReqCodeQuery(string ReqCode) : IRequest<Result<GetJobDescriptionResponseDto>>;