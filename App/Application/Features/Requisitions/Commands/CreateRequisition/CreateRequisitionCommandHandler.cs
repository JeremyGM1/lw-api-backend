using AutoMapper;
using MediatR;
using Application.Common.Results.RequisitionErrors;
using Application.Common.Results;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.Requisitions;
using Application.Common.Results.GenericErrors;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IGenericLogService<RequisitionLog> logger) : IRequestHandler<CreateRequisitionCommand, Result>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IGenericLogService<RequisitionLog> _logger = logger;

    public async Task<Result> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        if (await _reqRepository.GetRequisitionById(request.ReqCode) != null)
        {
            var reqError = RequisitionErrors.ExistingRequisition(request.ReqCode);
            await _logger.LogFailureAsync("N/A", "Create", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description);
            return Result.Failure(reqError);
        }

        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = userEmail;

        await _reqRepository.CreateAsync(reqToCreate);

        await _logger.LogSuccessAsync(reqToCreate.ReqCode, "Create", userEmail, ipAddress, userAgent, "Success");

        return Result.Success();
    }
}