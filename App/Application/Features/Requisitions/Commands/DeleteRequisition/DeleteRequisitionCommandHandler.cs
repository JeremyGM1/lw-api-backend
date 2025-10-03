using Application.Interfaces;
using AutoMapper;
using LoggingService.Interfaces;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;
public class DeleteRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<DeleteRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    //private readonly IAppLogger<DeleteRequisitionCommand> _logger = logger;
    public Task<Unit> Handle(DeleteRequisitionCommand request, CancellationToken cancellationToken)
    {
        //var userToDelele = _reqRepository.DeleteAsync(request.Id);
    }
}