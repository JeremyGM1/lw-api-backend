using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Microsoft.Extensions.Logging;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, ICommonHelpers commonHelpers, ILogger<CreateRequisitionCommandHandler> logger) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly ILogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating requisition {RequisitionId}", request.Id);

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            _commonHelpers.ThrowIfValidationFails(validationResult);

        _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId, request.LocationId, request.CompensationBandCurrencyId, request.CompensationBandIntervalId);

            var userEmail = _commonHelpers.GetCurrentUserFromToken();

            var reqToCreate = _mapper.Map<Requisition>(request);

            reqToCreate.CreatedAt = DateTime.UtcNow;
            reqToCreate.CreatedBy = userEmail;

            await _reqRepository.CreateAsync(reqToCreate);

            _logger.LogInformation("Requisition {Requisitionid} created successfully", request.Id);
            return Unit.Value;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to create requisition {RequisitionId}", request.Id);
            throw;
        }
    }
}