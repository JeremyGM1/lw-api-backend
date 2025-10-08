using System.ComponentModel.DataAnnotations;

namespace Application.Features.Common.Dtos.RequisitionDtos.Lookup;

public class GetReqHardwareDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Hardware { get; set; } = null!;
}