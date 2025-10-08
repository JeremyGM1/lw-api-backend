using MediatR;
using Application.Features.Common.Dtos.RequisitionDtos;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : DeleteRequisitionDto, IRequest<Unit>;