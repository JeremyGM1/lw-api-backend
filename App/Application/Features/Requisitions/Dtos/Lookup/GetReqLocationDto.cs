using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;
public class GetReqLocationDto
{
    [Required]
    public string Location { get; set; } = null!;
}