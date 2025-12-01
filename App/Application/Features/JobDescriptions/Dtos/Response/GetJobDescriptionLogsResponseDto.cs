namespace Application.Features.JobDescriptions.Dtos.Response;
public class GetJobDescriptionLogsResponseDto
{
    public string EntityId { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string TimeStamp { get; set; } = null!;
}