using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqIntervalDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Interval { get; set; } = null!;
}