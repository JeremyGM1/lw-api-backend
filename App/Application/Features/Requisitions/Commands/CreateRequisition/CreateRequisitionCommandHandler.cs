using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Requisitions;
using LoggingService.Interfaces;
using MediatR;

namespace Application.Features.Requisitions.Commands.CreateRequisition;
public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    //private readonly IAppLogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = "Random user";

        await _reqRepository.CreateAsync(reqToCreate);

        return Unit.Value;
    }
}