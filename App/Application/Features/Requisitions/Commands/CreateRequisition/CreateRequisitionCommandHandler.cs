using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, ICommonHelpers handlerHelpers) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly ICommonHelpers _handlerHelpers = handlerHelpers;
    //private readonly IAppLogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }

        _handlerHelpers.ValidateForeighKey(request.StatusId, request.WorkTypeId, request.HardwareId);

        var userEmail = _handlerHelpers.GetCurrentUserFromToken();

        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = userEmail;

        await _reqRepository.CreateAsync(reqToCreate);

        return Unit.Value;
    }
}