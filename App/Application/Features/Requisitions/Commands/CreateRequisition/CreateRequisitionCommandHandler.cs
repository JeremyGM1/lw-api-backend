using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Application.Common.Helpers;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, ICommonHelpers commonHelpers) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    //private readonly IAppLogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        _commonHelpers.ThrowIfValidationFails(validationResult);

        _commonHelpers.ValidateForeignKey(request.StatusId, request.WorkTypeId, request.HardwareId);

        var userEmail = _commonHelpers.GetCurrentUserFromToken();

        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = userEmail;

        await _reqRepository.CreateAsync(reqToCreate);

        return Unit.Value;
    }
}