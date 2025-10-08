using AutoMapper;
using FluentValidation;
using MediatR;
using Application.Exceptions;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<DeleteRequisitionCommand> validator) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<DeleteRequisitionCommand> _validator = validator;

    //private readonly IAppLogger<DeleteRequisitionCommand> _logger = logger;
    public async Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetByIdAsync(request.Id);
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }

        await _reqRepository.DeleteAsync(requisition);

        return Unit.Value;
    }
}