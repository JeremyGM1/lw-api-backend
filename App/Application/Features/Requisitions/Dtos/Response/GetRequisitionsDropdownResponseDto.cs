namespace Application.Features.Requisitions.Dtos.Response;
public class GetRequisitionsDropdownResponseDto
{
    public int Id { get; set; }
    public string ReqCode { get; set; } = null!;
    public string Name { get; set; } = null!;
}