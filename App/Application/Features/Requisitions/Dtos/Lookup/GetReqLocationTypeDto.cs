using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqLocationTypeDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string LocationType { get; set; } = null!;
}