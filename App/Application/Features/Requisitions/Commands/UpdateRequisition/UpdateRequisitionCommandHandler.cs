using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Common.Helpers;
using Application.Exceptions;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<UpdateRequisitionCommand> validator, ICommonHelpers commonHelpers, ILoggerHelpers loggerHelpers) : IRequestHandler<UpdateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<UpdateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly ILoggerHelpers _loggerHelpers = loggerHelpers;

    public async Task<Unit> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _commonHelpers.GetCurrentUserFromToken();
        var ipAddress = _commonHelpers.GetUserIpAddress();
        var userAgent = _commonHelpers.GetUserAgent();

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            _commonHelpers.ThrowIfValidationFails(validationResult);

            var requisition = await _reqRepository.GetRequisitionById(request.Id!);
            if (requisition == null)
                throw new NotFoundException(nameof(requisition), request.Id!);

            _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId);

            _mapper.Map(request, requisition);

            requisition.CreatedAt = requisition!.CreatedAt;
            requisition.CreatedBy = requisition!.CreatedBy;
            requisition.UpdatedAt = DateTime.UtcNow;
            requisition.UpdatedBy = userEmail;

            await _reqRepository.UpdateAsync(requisition);

            await _loggerHelpers.LogRequisitionAsync(requisition.Id, "Update", userEmail, ipAddress, userAgent, "Success");

            return Unit.Value;
        }
        catch (Exception ex)
        {
            await _loggerHelpers.LogRequisitionAsync(null!, "Update", userEmail, ipAddress, userAgent, "Failure", ex.Message);
            throw;
        }
    }
}