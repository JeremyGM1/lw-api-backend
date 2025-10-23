using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, ICommonHelpers commonHelpers, ILoggerHelpers loggerHelpers) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly ILoggerHelpers _loggerHelpers = loggerHelpers;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _commonHelpers.GetCurrentUserFromToken();
        var ipAddress = _commonHelpers.GetUserIpAddress();
        var userAgent = _commonHelpers.GetUserAgent();

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            _commonHelpers.ThrowIfValidationFails(validationResult);

            _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId);

            var reqToCreate = _mapper.Map<Requisition>(request);

            reqToCreate.CreatedAt = DateTime.UtcNow;
            reqToCreate.CreatedBy = userEmail;

            await _reqRepository.CreateAsync(reqToCreate);

            await _loggerHelpers.LogRequisitionAsync(reqToCreate.Id, "Create", userEmail, ipAddress, userAgent, "Success");

            return Unit.Value;
        }
        catch (Exception ex)
        {
            await _loggerHelpers.LogRequisitionAsync(request.Id, "Create", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            throw;
        }
    }
}