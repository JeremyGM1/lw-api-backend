using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Features.Requisitions.Dtos;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Queries.GetRequisitionById;

public class GetRequisitionByIdQueryHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<GetRequisitionByIdQuery> validator, ICommonHelpers commonHelpers) : IRequestHandler<GetRequisitionByIdQuery, GetRequisitionDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<GetRequisitionByIdQuery> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;

    public async Task<GetRequisitionDto> Handle(GetRequisitionByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        _commonHelpers.ThrowIfValidationFails(validationResult);

        var requisition = await _reqRepository.GetRequisitionById(request.Id);

        if (requisition == null)
            throw new NotFoundException(nameof(requisition), request.Id);

        return _mapper.Map<GetRequisitionDto>(requisition);
    }
}