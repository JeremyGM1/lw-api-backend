using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, IValidator<DeleteRequisitionCommand> validator, ICommonHelpers handlerHelpers) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<DeleteRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _handlerHelpers = handlerHelpers;

    //private readonly IAppLogger<DeleteRequisitionCommand> _logger = logger;
    public async Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetRequisitionById(request.Id);
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }

        _handlerHelpers.ValidateEntityExist(requisition, request.Id);

        await _reqRepository.DeleteAsync(requisition!);

        return Unit.Value;
    }
}