using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommand : IRequest<Unit>
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public required string Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; set; }

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
    public string? Location { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Department { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? HiringManager { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Owner { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be 3-letter ISO code")]
    public string? CompensationBandCurrency { get; set; }

    [Required]
    [StringLength(10)]
    public string? CompensationBandInterval { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }
}