using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Results.RequisitionErrors;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, ILoggerService logger) : IRequestHandler<UpdateRequisitionCommand, Result>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly ILoggerService _logger = logger;

    public async Task<Result> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
            var requisition = await _reqRepository.GetRequisitionById(request.ReqCode!);
            if (requisition == null)
            {
                var reqError = RequisitionErrors.NotExistingRequisition(request.ReqCode!);
                await _logger.LogFailureAsync("N/A", "Delete", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description);
                return Result.Failure(RequisitionErrors.NotExistingRequisition(request.ReqCode!));
            }

            _mapper.Map(request, requisition);

            requisition.CreatedAt = requisition!.CreatedAt;
            requisition.CreatedBy = requisition!.CreatedBy;
            requisition.UpdatedAt = DateTime.UtcNow;
            requisition.UpdatedBy = userEmail;

            await _reqRepository.UpdateAsync(requisition);

            await _logger.LogSuccessAsync(requisition.ReqCode, "Update", userEmail, ipAddress, userAgent, "Success");

            return Result.Success();
        }
        catch (Exception ex)
        {
            var errorDescription = $"{ex.Message}" + (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");
            await _logger.LogFailureAsync(null!, "Update", userEmail, ipAddress, userAgent, "Failure", ex.Message, errorDescription);
            return Result.Failure(RequisitionErrors.RequisitionException(ex.Message));
        }
    }
}