namespace Application.Features.Requisitions.Dtos;

public class GetRequisitionDto
{
    public string Id { get; set; } = null!;

    public string ReqCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string StatusName { get; set; } = null!;

    public string WorkTypeName { get; set; } = null!;

    public string HardwareName { get; set; } = null!;
    public string LocationType { get; set; } = null!;
    public int Headcount { get; set; }

    public string Location { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string HiringManager { get; set; } = null!;

    public string Owner { get; set; } = null!;

    public decimal CompensationBandMin { get; set; }

    public decimal CompensationBandMax { get; set; }

    public string CompensationBandCurrency { get; set; } = null!;

    public string CompensationBandInterval { get; set; } = null!;

    public string? Description { get; set; }

    public string? BackfillName { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}