using System.ComponentModel.DataAnnotations;
using Application.Common.Results;
using MediatR;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : IRequest<Result>
{
    [Required]
    [StringLength(6)]
    public required string ReqCode { get; set; }
}