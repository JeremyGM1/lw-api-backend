using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqStatusesDto
{

    [Required]
    public string Status { get; set; } = null!;
}