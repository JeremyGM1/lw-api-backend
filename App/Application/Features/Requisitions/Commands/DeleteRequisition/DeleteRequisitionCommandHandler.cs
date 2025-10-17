using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, IValidator<DeleteRequisitionCommand> validator, ICommonHelpers commonHelpers) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<DeleteRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    //private readonly IAppLogger<DeleteRequisitionCommand> _logger = logger;

    public async Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        _commonHelpers.ThrowIfValidationFails(validationResult);

        var requisition = await _reqRepository.GetRequisitionById(request.Id);
        if (requisition == null)
            throw new NotFoundException(nameof(requisition), request.Id);

        await _reqRepository.DeleteAsync(requisition!);

        return Unit.Value;
    }
}