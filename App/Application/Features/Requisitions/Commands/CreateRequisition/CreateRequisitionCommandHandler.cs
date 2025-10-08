using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Features.Requisitions.Commands.CreateRequisition;

public class CreateRequisitionCommandHandler(IMapper mapper, IRequisitionRepository reqRepository, IValidator<CreateRequisitionCommand> validator, IHttpContextAccessor httpContext) : IRequestHandler<CreateRequisitionCommand, Unit>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly IValidator<CreateRequisitionCommand> _validator = validator;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    //private readonly IAppLogger<CreateRequisitionCommandHandler> _logger = logger;

    public async Task<Unit> Handle(CreateRequisitionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }

        var statusExist = await _reqRepository.ForeignKeyExist<RequisitionStatus>(request.StatusId);
        var workTypestatusExist = await _reqRepository.ForeignKeyExist<RequisitionWorkType>(request.WorkTypeId);
        var hardwareExist = await _reqRepository.ForeignKeyExist<RequisitionHardware>(request.HardwareId);

        if (!statusExist || !workTypestatusExist || !hardwareExist)
        {
            var errors = new Dictionary<string, string[]>();
            if (!statusExist) errors.Add("StatusId", ["Status not found"]);
            if (!workTypestatusExist) errors.Add("StatusId", ["Status not found"]);
            if (!hardwareExist) errors.Add("StatusId", ["Status not found"]);

            throw new ForeignKeyNotFoundException("Invalid foreign key", errors);
        }

        var user = _httpContext.HttpContext?.User;
        var userEmail = user?.Identity?.Name ?? user?.FindFirst(ClaimTypes.Email)?.Value ?? "System";

        var reqToCreate = _mapper.Map<Requisition>(request);

        reqToCreate.CreatedAt = DateTime.UtcNow;
        reqToCreate.CreatedBy = userEmail;

        await _reqRepository.CreateAsync(reqToCreate);

        return Unit.Value;
    }
}