using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;

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
        _commonHelpers.ThrowIfValidationFails(validationResult);

        var requisition = await _reqRepository.GetRequisitionById(request.Id!);
        if (requisition == null)
            throw new NotFoundException(nameof(requisition), request.Id!);

        _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId);

        var userEmail = _commonHelpers.GetCurrentUserFromToken();
        _mapper.Map(request, requisition);

        requisition.CreatedAt = requisition!.CreatedAt;
        requisition.CreatedBy = requisition!.CreatedBy;
        requisition.UpdatedAt = DateTime.UtcNow;
        requisition.UpdatedBy = userEmail;

        await _reqRepository.UpdateAsync(requisition);

        return Unit.Value;
    }
}