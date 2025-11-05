using Application.Common.Results;
using Application.Common.Results.RequisitionErrors;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IRequisitionLogService logger) : IRequestHandler<DeleteRequisitionCommand, Result>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IRequisitionLogService _logger = logger;

    public async Task<Result> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
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