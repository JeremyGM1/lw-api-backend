using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Common.Configs;
using Application.Common.Results;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public record UpdateRequisitionCommand : IRequest<Result>
{
    [Required]
    [StringLength(6)]
    public string? ReqCode { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.NonRequiredStringMinLength)]
    public string? Name { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int StatusId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int WorkTypeId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int HardwareId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int Headcount { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int LocationTypeId { get; set; }

    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int LocationId { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.NonRequiredStringMinLength)]
    public string? Department { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.NonRequiredStringMinLength)]
    public string? HiringManager { get; set; }

    [StringLength(100, MinimumLength = ValidationDefaultValues.NonRequiredStringMinLength)]
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