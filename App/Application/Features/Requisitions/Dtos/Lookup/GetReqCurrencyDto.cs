using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos.Lookup;

public class GetReqCurrencyDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Currency { get; set; } = null!;
}