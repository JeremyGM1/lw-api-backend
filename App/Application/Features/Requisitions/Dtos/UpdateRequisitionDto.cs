using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos;

public class UpdateRequisitionDto
{
    public string? Name { get; set; }

    public int StatusId { get; set; }

    public int WorkTypeId { get; set; }

    public int HardwareId { get; set; }

    public int Headcount { get; set; }

    public string? Location { get; set; }

    public string? Department { get; set; }

    public string? HiringManager { get; set; }

    public string? Owner { get; set; }

    public decimal CompensationBandMin { get; set; }

    public decimal CompensationBandMax { get; set; }

    public string? CompensationBandCurrency { get; set; }

    public string? CompensationBandInterval { get; set; }

    public string? Description { get; set; }

    public string? BackfillName { get; set; }
}