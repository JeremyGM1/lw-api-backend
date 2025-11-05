using AutoMapper;
using MediatR;
using Application.Common.Results.RequisitionErrors;
using Application.Common.Results;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, ILoggerService logger) : IRequestHandler<CreateRequisitionCommand, Result>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly ILoggerService _logger = logger;

    public async Task<Result> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
            if (await _reqRepository.RequisitionExists(request.ReqCode))
                return Result.Failure(RequisitionErrors.ExistingRequisition(request.ReqCode));

            await _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId, request.LocationId, request.CompensationBandCurrencyId, request.CompensationBandIntervalId);

            var reqToCreate = _mapper.Map<Requisition>(request);

            reqToCreate.CreatedAt = DateTime.UtcNow;
            reqToCreate.CreatedBy = userEmail;

            await _reqRepository.CreateAsync(reqToCreate);

            await _logger.LogSuccessAsync(reqToCreate.ReqCode, "Create", userEmail, ipAddress, userAgent, "Success");

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _logger.LogFailureAsync(request.ReqCode, "Create", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            return Result.Failure(RequisitionErrors.RequisitionException(ex.Message));
        }
    }
}