using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqHardwareDto
{

    [Required]
    public string Hardware { get; set; } = null!;
}