using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;

namespace Application.Common.Helpers;

public class CommonHelpers(IHttpContextAccessor httpContextAccessor, IRequisitionRepository reqRepository) : ICommonHelpers
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public string GetCurrentUserFromToken()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity?.Name ?? user?.FindFirst(ClaimTypes.Email)?.Value ?? "System";
    }

    public void ValidateEntityExist<T>(T entity, string id)
    {
        if (entity == null)
            throw new NotFoundException(typeof(T).Name, id);
    }

    public async void ValidateForeighKey(int statusId, int workTypeId, int hardwareId)
    {
        var statusExist = await _reqRepository.ForeignKeyExist<RequisitionStatus>(statusId);
        var workTypestatusExist = await _reqRepository.ForeignKeyExist<RequisitionWorkType>(workTypeId);
        var hardwareExist = await _reqRepository.ForeignKeyExist<RequisitionHardware>(hardwareId);

        if (!statusExist || !workTypestatusExist || !hardwareExist)
        {
            var errors = new Dictionary<string, string[]>();
            if (!statusExist) errors.Add("StatusId", ["Status not found"]);
            if (!workTypestatusExist) errors.Add("StatusId", ["Status not found"]);
            if (!hardwareExist) errors.Add("StatusId", ["Status not found"]);

            throw new ForeignKeyNotFoundException("Invalid foreign key", errors);
        }
    }
}