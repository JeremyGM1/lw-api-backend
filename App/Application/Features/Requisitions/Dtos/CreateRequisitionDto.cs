using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos;

public class CreateRequisitionDto
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public int StatusId { get; set; }

    public int WorkTypeId { get; set; }

    public int HardwareId { get; set; }

    public int Headcount { get; set; }

    public required string Location { get; set; }

    public required string Department { get; set; }

    public required string HiringManager { get; set; }

    public required string Owner { get; set; }

    public decimal CompensationBandMin { get; set; }

    public decimal CompensationBandMax { get; set; }

    public required string CompensationBandCurrency { get; set; }

    public required string CompensationBandInterval { get; set; }

    public string? Description { get; set; }

    public string? BackfillName { get; set; }
}