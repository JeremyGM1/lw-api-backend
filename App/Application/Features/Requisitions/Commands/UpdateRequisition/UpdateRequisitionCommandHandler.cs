using AutoMapper;
using MediatR;
using Application.Common.Results;
using Application.Common.Results.RequisitionErrors;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IRequisitionLogService logger) : IRequestHandler<UpdateRequisitionCommand, Result>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IRequisitionLogService _logger = logger;

    public async Task<Result> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
            var requisition = await _reqRepository.GetRequisitionById(request.ReqCode!);
            if (requisition == null)
                return Result.Failure(RequisitionErrors.NotExistingRequisition(request.ReqCode!));

            await _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId, request.LocationId, request.CompensationBandCurrencyId, request.CompensationBandIntervalId);

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
            await _logger.LogFailureAsync(null!, "Update", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            return Result.Failure(RequisitionErrors.RequisitionException(ex.Message));
        }
    }
}