using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;
using FluentValidation;
using MediatR;

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

        _handlerHelpers.ValidateIfEntityExist(requisition, request.Id);

        await _reqRepository.DeleteAsync(requisition!);

        return Unit.Value;
    }
}