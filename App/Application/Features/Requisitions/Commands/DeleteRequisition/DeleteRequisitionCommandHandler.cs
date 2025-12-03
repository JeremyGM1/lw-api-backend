using Application.Common.Results;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.RequisitionErrors;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IGenericLogService<RequisitionLog> logger) : IRequestHandler<DeleteRequisitionCommand, Result>
{
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IGenericLogService<RequisitionLog> _logger = logger;

    public async Task<Result> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        var requisition = await _reqRepository.GetRequisitionById(request.ReqCode);
        if (requisition == null)
        {
            var reqError = RequisitionErrors.NotExistingRequisition(request.ReqCode);
            await _logger.LogFailureAsync("N/A", "Delete", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description);
            return Result.Failure(reqError);
        }

        await _logger.LogSuccessAsync(requisition.ReqCode, "Delete", userEmail, ipAddress, userAgent, "Success");

        await _reqRepository.DeleteAsync(requisition!);

        return Result.Success();
    }
}