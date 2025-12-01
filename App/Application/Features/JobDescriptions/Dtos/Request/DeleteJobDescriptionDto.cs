using System.ComponentModel.DataAnnotations;

namespace Application.Features.JobDescriptions.Dtos.Request;
public class DeleteJobDescriptionDto
{
    [Required]
    public int Id { get; set; }
}