using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Requisitions;
using FluentValidation;
using MediatR;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<UpdateRequisitionCommand> validator, ICommonHelpers commonHelpers) : IRequestHandler<UpdateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<UpdateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;

    public async Task<Unit> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var requisition = await _reqRepository.GetByIdAsync(request.Id!);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition update", validationResult);
        }

        _commonHelpers.ValidateForeighKey(request.StatusId, request.WorkTypeId, request.HardwareId);

        _commonHelpers.ValidateIfEntityExist(requisition, request.Id!);

        var userEmail = _commonHelpers.GetCurrentUserFromToken();

        var reqToUpdate = _mapper.Map<Requisition>(request);

        reqToUpdate.CreatedAt = requisition!.CreatedAt;
        reqToUpdate.CreatedBy = requisition!.CreatedBy;
        reqToUpdate.UpdatedAt = DateTime.UtcNow;
        reqToUpdate.UpdatedBy = userEmail;

        await _reqRepository.UpdateAsync(reqToUpdate);

        return Unit.Value;
    }
}