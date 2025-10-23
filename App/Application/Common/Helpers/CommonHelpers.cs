using System.Security.Claims;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Requisitions;
using LoggingService.Interfaces;

namespace Application.Common.Helpers;

public class CommonHelpers(IHttpContextAccessor httpContextAccessor, IRequisitionRepository reqRepository) : ICommonHelpers
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public string GetCurrentUserFromToken()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var email = user?.Identity?.Name ?? user?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            throw new UnauthorizedAccessException("User not authenticated");

        return email;
    }

    public string GetUserIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',').First().Trim();
        }

        var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    public string GetUserAgent()
    {
        return _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString() ?? "Unknown";
    }

    public void ThrowIfValidationFails(ValidationResult validationResult)
    {
        if (validationResult.Errors.Count != 0)
        {
            throw new BadRequestException("Invalid requisition", validationResult);
        }
    }

    public async void ValidateForeignKey(int statusId, int workTypeId, int hardwareId)
    {
        var statusExist = await _reqRepository.ForeignKeyExist<RequisitionStatus>(statusId);
        var workTypestatusExist = await _reqRepository.ForeignKeyExist<RequisitionWorkType>(workTypeId);
        var hardwareExist = await _reqRepository.ForeignKeyExist<RequisitionHardware>(hardwareId);

        if (!statusExist || !workTypestatusExist || !hardwareExist)
        {
            var errors = new Dictionary<string, string[]>();
            if (!statusExist) errors.Add("StatusId", ["Status not found"]);
            if (!workTypestatusExist) errors.Add("WorkTypeId", ["WorkType not found"]);
            if (!hardwareExist) errors.Add("HardwareId", ["hardware not found"]);

            throw new ForeignKeyNotFoundException("Invalid foreign key", errors);
        }
    }
}