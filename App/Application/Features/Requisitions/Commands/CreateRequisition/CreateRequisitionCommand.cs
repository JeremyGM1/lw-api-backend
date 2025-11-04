using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Common.Configs;
using Application.Common.Result;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommand : IRequest<Result>
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public required string ReqCode { get; set; }

    [Required]
    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public required string Name { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int StatusId { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int WorkTypeId { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int HardwareId { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int LocationTypeId { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int Headcount { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int LocationId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public required string Department { get; set; }

    [Required]
    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public required string HiringManager { get; set; }

    [Required]
    [StringLength(100, MinimumLength = ValidationDefaultValues.StringMinLength)]
    public required string Owner { get; set; }

    [Required]
    [Range(0, (double)ValidationDefaultValues.CompensationBandMaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Required]
    [Range(0, (double)ValidationDefaultValues.CompensationBandMaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int CompensationBandCurrencyId { get; set; }

    [Required]
    [Range(1, ValidationDefaultValues.IntMaxValue)]
    public int CompensationBandIntervalId { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }
}