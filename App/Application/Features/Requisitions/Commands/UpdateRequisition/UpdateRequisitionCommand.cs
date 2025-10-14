using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public record UpdateRequisitionCommand : IRequest<Unit>
{
    public string? Id { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; set; }

    [Range(1, int.MaxValue)]
    public int StatusId { get; set; }

    [Range(1, int.MaxValue)]
    public int WorkTypeId { get; set; }

    [Range(1, int.MaxValue)]
    public int HardwareId { get; set; }

    [Range(1, int.MaxValue)]
    public int Headcount { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Location { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Department { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? HiringManager { get; set; }

    [StringLength(100, MinimumLength = 1)]
    public string? Owner { get; set; }

    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMin { get; set; }

    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal CompensationBandMax { get; set; }

    [StringLength(3, MinimumLength = 3)]
    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be 3-letter ISO code")]
    public string? CompensationBandCurrency { get; set; }

    [StringLength(10)]
    public string? CompensationBandInterval { get; set; }

    [StringLength(200)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? BackfillName { get; set; }
}