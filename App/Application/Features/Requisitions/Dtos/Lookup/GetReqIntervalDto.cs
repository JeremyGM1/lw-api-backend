using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;
public class GetReqIntervalDto
{
    [Required]
    public string Interval { get; set; } = null!;
}