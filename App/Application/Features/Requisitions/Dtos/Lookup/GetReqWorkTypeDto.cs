using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqWorkTypeDto
{

    [Required]
    public string WorkType { get; set; } = null!;
}