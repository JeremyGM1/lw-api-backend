using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Features.Common.HandlerHelpers;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<UpdateRequisitionCommand> validator, CommonHelpers handlerHelpers) : IRequestHandler<UpdateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<UpdateRequisitionCommand> _validator = validator;
    private readonly CommonHelpers _handlerHelpers = handlerHelpers;

    public async Task<Unit> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var requisition = await _reqRepository.GetByIdAsync(request.Id!);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition update", validationResult);
        }

        _handlerHelpers.ValidateForeignKeys(request.StatusId, request.WorkTypeId, request.HardwareId);

        _handlerHelpers.ValidateEntityExist(requisition, request.Id!);

        var userEmail = _handlerHelpers.GetCurrentUserFromToken();

        var reqToUpdate = _mapper.Map<Requisition>(request);

        reqToUpdate.CreatedAt = requisition!.CreatedAt;
        reqToUpdate.CreatedBy = requisition!.CreatedBy;
        reqToUpdate.UpdatedAt = DateTime.UtcNow;
        reqToUpdate.UpdatedBy = userEmail;

        await _reqRepository.UpdateAsync(reqToUpdate);

        return Unit.Value;
    }
}