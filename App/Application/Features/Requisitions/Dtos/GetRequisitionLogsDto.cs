namespace Application.Features.Requisitions.Dtos;

public class GetRequisitionLogsDto
{
    public string EntityId { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string TimeStamp { get; set; } = null!;
}