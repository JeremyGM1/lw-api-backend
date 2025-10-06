using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : IRequest<Unit>
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Id { get; set; } = null!;
}