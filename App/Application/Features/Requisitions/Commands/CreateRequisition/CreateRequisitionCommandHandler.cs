using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using Application.Features.Common.HandlerHelpers;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, CommonHelpers handlerHelpers) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly CommonHelpers _handlerHelpers = handlerHelpers;
    //private readonly IAppLogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }

        _handlerHelpers.ValidateForeignKeys(request.StatusId, request.WorkTypeId, request.HardwareId);

        var userEmail = _handlerHelpers.GetCurrentUserFromToken();

        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = userEmail;

        await _reqRepository.CreateAsync(reqToCreate);

        return Unit.Value;
    }
}