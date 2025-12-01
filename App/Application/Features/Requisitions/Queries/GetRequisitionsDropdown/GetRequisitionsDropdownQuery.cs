using Application.Common.Results;
using Application.Features.Requisitions.Dtos.Response;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetRequisitionsDropdown;
public class GetRequisitionsDropdownQuery : IRequest<Result<List<GetRequisitionsDropdownResponseDto>>>;