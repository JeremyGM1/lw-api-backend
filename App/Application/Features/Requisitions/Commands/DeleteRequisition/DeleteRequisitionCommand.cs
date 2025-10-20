using System.ComponentModel.DataAnnotations;
using Application.Features.Requisitions.Dtos;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : IRequest<Unit>
{
    [Required]
    [StringLength(6)]
    public required string Id { get; set; }
}