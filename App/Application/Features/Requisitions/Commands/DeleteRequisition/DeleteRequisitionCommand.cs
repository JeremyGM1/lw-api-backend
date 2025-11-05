using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Common.Results;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : IRequest<Result>
{
    [Required]
    [StringLength(6)]
    public required string ReqCode { get; set; }
}