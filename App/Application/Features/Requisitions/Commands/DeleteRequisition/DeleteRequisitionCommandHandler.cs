using Application.Common.Result;
using Application.Exceptions;
using Application.Features.Requisitions.Errors;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using FluentValidation;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, IValidator<DeleteRequisitionCommand> validator, ICommonHelpers commonHelpers, IRequisitionLogService logger) : IRequestHandler<DeleteRequisitionCommand, Result>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<DeleteRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IRequisitionLogService _logger = logger;

    public async Task<Result> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _commonHelpers.GetCurrentUserFromToken();
        var ipAddress = _commonHelpers.GetUserIpAddress();
        var userAgent = _commonHelpers.GetUserAgent();

        try
        {
            if (await _reqRepository.RequisitionExists(request.ReqCode))
                return Result.Failure(RequisitionErrors.NotExistingRequisition(request.ReqCode));

            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //_commonHelpers.ThrowIfValidationFails(validationResult);

            var requisition = await _reqRepository.GetRequisitionById(request.ReqCode);
            if (requisition == null)
                return Result.Failure(RequisitionErrors.NotExistingRequisition(request.ReqCode));

            await _logger.LogSuccessAsync(requisition.ReqCode, "Delete", userEmail, ipAddress, userAgent, "Success");

            await _reqRepository.DeleteAsync(requisition!);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _logger.LogFailureAsync(null!, "Delete", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            return Result.Failure(RequisitionErrors.RequisitionException(ex.Message));
        }
    }
}