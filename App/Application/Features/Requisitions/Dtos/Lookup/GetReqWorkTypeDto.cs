using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqWorkTypeDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string WorkType { get; set; } = null!;
}