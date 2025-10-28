using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Domain.Entities.Requisitions;
using Application.Interfaces.LoggingService;
using Application.Interfaces.Features.Requisitions;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, ICommonHelpers commonHelpers, IRequisitionLogService logger) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IRequisitionLogService _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            _commonHelpers.ThrowIfValidationFails(validationResult);

        _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId, request.LocationId, request.CompensationBandCurrencyId, request.CompensationBandIntervalId);

            var reqToCreate = _mapper.Map<Requisition>(request);

            reqToCreate.CreatedAt = DateTime.UtcNow;
            reqToCreate.CreatedBy = userEmail;

            await _reqRepository.CreateAsync(reqToCreate);

            await _logger.LogSuccessAsync(reqToCreate.Id, "Create", userEmail, ipAddress, userAgent, "Success");

            return Unit.Value;
        }
        catch (Exception ex)
        {
            await _logger.LogFailureAsync(request.Id, "Create", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            throw;
        }
    }
}