using MediatR;
using Application.Features.Requisitions.Dtos;

namespace Application.Features.Requisitions.Commands.DeleteRequisition;

public class DeleteRequisitionCommand : DeleteRequisitionDto, IRequest<Unit>;