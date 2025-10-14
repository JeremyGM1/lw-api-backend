using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos;

public class DeleteRequisitionDto
{
    [Required]
    [StringLength(6)]
    public required string Id { get; set; }
}