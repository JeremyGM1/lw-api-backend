using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqStatusesDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Status { get; set; } = null!;
}