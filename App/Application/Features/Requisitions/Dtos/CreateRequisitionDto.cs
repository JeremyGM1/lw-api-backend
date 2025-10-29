namespace Application.Features.Requisitions.Dtos;

public class CreateRequisitionDto
{
    public required int Id { get; set; }
    public required string ReqCode { get; set; }

    public required string Name { get; set; }

    public int StatusId { get; set; }

    public int WorkTypeId { get; set; }

    public int HardwareId { get; set; }

    public int Headcount { get; set; }

    //public required string Location { get; set; }
    public int Locationid { get; set; }

    public required string Department { get; set; }

    public required string HiringManager { get; set; }

    public required string Owner { get; set; }

    public decimal CompensationBandMin { get; set; }

    public decimal CompensationBandMax { get; set; }

    public int CompensationBandCurrencyId { get; set; }

    public int CompensationBandIntervalId { get; set; }

    public string? Description { get; set; }

    public string? BackfillName { get; set; }
}