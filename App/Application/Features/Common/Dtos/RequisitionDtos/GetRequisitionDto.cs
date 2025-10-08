using System.ComponentModel.DataAnnotations;

namespace Application.Features.Common.Dtos.RequisitionDtos;

public class GetRequisitionDto
{
    [StringLength(6, MinimumLength = 6)]
    public string Id { get; set; } = null!;

    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int WorkTypeId { get; set; }

    public string WorkTypeName { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int HardwareId { get; set; }

    public string HardwareName { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Headcount { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string Location { get; set; } = null!;

    [StringLength(100, MinimumLength = 1)]
    public string Department { get; set; } = null!;

    [StringLength(100, MinimumLength = 1)]
    public string HiringManager { get; set; } = null!;

    [StringLength(100, MinimumLength = 1)]
    public string Owner { get; set; } = null!;

    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [StringLength(3, MinimumLength = 3)] // ISO 4217
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be 3-letter ISO code")]
    public string CompensationBandCurrency { get; set; } = null!;

    [StringLength(10)]
    public string CompensationBandInterval { get; set; } = null!;

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? ApprovalDate { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; } = default!;

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
}