using System.ComponentModel.DataAnnotations;

namespace Application.Features.Common.Dtos.RequisitionDtos;

public class DeleteRequisitionDto
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public required string Id { get; set; }
}