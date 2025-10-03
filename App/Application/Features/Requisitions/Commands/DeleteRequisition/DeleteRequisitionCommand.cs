using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;
public class DeleteRequisitionCommand : IRequest<Unit>
{
    public string Id { get; set; } = null!;
}