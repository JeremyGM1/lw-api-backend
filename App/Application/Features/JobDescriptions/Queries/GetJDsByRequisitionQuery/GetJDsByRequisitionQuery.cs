using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using MediatR;

namespace Application.Features.JobDescriptions.Queries.GetJDsByRequisitionQuery;
public record GetJDsByRequisitionQuery(string ReqCode) : IRequest<Result<IEnumerable<GetJobDescriptionResponseDto>>>;