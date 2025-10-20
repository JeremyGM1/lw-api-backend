using System.ComponentModel.DataAnnotations;

namespace Application.Features.Requisitions.Dtos;

public class DeleteRequisitionDto
{
    public required string Id { get; set; }
}