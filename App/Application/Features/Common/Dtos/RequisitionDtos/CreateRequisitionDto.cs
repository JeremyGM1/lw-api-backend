using System.ComponentModel.DataAnnotations;

namespace Application.Features.Common.Dtos.RequisitionDtos;

public class CreateRequisitionDto
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public required string Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int StatusId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int WorkTypeId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int HardwareId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Headcount { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Location { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Department { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string HiringManager { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Owner { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)] // ISO 4217
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be 3 Uppercase letters ISO code")]
    public required string CompensationBandCurrency { get; set; }

    [Required]
    [StringLength(10)]
    public required string CompensationBandInterval { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }
}