using System.ComponentModel.DataAnnotations;
using Application.Common.Configs;

using MediatR;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public record UpdateRequisitionCommand : IRequest<Unit>
{
    [Required]
    [StringLength(6)]
    public string? ReqCode { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public string? Name { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int StatusId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int WorkTypeId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int HardwareId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int Headcount { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int LocationId { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public string? Department { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public string? HiringManager { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public string? Owner { get; set; }

    [Range(0, (double)ValidationDefaultValues.CompensationBandMaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Range(0, (Double)ValidationDefaultValues.CompensationBandMaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int CompensationBandCurrencyId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int CompensationBandIntervalId { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }
}