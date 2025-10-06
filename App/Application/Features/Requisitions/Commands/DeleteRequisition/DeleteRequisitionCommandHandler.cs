using AutoMapper;
using MediatR;
using Application.Interfaces;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;
public class DeleteRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    //private readonly IAppLogger<DeleteRequisitionCommand> _logger = logger;
    public async Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetByIdAsync(request.Id);

        await _reqRepository.DeleteAsync(requisition);

        return Unit.Value;
    }
}