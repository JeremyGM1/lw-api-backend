using AutoMapper;
using MediatR;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository) : IRequestHandler<UpdateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public async Task<Unit> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var requisition = await _reqRepository.GetByIdAsync(request.Id);

        var reqToUpdate = _mapper.Map<Requisition>(request);

        reqToUpdate.CreatedAt = requisition.CreatedAt;
        reqToUpdate.CreatedBy = requisition.CreatedBy;
        reqToUpdate.UpdatedAt = DateTime.UtcNow;
        reqToUpdate.UpdatedBy = "Update User";

        await _reqRepository.UpdateAsync(reqToUpdate);

        return Unit.Value;
    }
}