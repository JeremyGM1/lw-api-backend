using Application.Common.Results;
using Application.Features.Requisitions.Dtos;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetReguisitionLogs;
public record GetRequisitionLogsByIdQuery(string ReqCode) : IRequest<Result<List<GetRequisitionLogsDto>>>;