using Application.Features.Shared.Dtos.RequisitionDtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;
public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    Task<GetRequisitionDto> IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>.Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}