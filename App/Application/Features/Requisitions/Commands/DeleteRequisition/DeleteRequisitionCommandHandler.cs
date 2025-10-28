using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.LoggingService;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, IValidator<DeleteRequisitionCommand> validator, ICommonHelpers commonHelpers, IRequisitionLogService logger) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<DeleteRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IRequisitionLogService _logger = logger;

    public async Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _commonHelpers.GetCurrentUserFromToken();
        var ipAddress = _commonHelpers.GetUserIpAddress();
        var userAgent = _commonHelpers.GetUserAgent();

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            _commonHelpers.ThrowIfValidationFails(validationResult);

            var requisition = await _reqRepository.GetRequisitionById(request.Id);
            if (requisition == null)
                throw new NotFoundException(nameof(requisition), request.Id);

            await _logger.LogSuccessAsync(requisition.Id, "Delete", userEmail, ipAddress, userAgent, "Success");

            await _reqRepository.DeleteAsync(requisition!);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            await _logger.LogFailureAsync(null!, "Delete", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            throw;
        }
    }
}