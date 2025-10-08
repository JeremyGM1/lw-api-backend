using System.Security.Claims;
using AutoMapper;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.UpdateRequisition;

public class UpdateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<UpdateRequisitionCommand> validator, IHttpContextAccessor httpContext) : IRequestHandler<UpdateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<UpdateRequisitionCommand> _validator = validator;
    private readonly IHttpContextAccessor _httpContext = httpContext;

    public async Task<Unit> Handle(UpdateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        // TODO: Validate if foreign key exist

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition update", validationResult);
        }

        var user = _httpContext.HttpContext?.User;
        var userEmail = user?.Identity?.Name ?? user?.FindFirst(ClaimTypes.Email)?.Value ?? "System";

        var requisition = await _reqRepository.GetByIdAsync(request.Id!);

        var reqToUpdate = _mapper.Map<Requisition>(request);

        reqToUpdate.CreatedAt = requisition.CreatedAt;
        reqToUpdate.CreatedBy = requisition.CreatedBy;
        reqToUpdate.UpdatedAt = DateTime.UtcNow;
        reqToUpdate.UpdatedBy = userEmail;

        await _reqRepository.UpdateAsync(reqToUpdate);

        return Unit.Value;
    }
}