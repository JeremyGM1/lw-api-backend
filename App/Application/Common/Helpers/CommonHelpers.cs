using System.Security.Claims;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Domain.Entities.Requisitions;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.Features.Common;
using Application.Common.Results;
using Application.Common.Results.GenericErrors;

namespace Application.Common.Helpers;

public class CommonHelpers(IHttpContextAccessor httpContextAccessor, IRequisitionRepository reqRepository) : ICommonHelpers
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRequisitionRepository _reqRepository = reqRepository;

    public string GetCurrentUserFromToken()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var email = user?.Identity?.Name ?? user?.FindFirst(ClaimTypes.Email)?.Value ?? "unknown-user@email.com";

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

    public async Task<Result> ValidateForeignKey(int statusId, int workTypeId, int hardwareId, int locationId, int currencyId, int intervalId)
    {
        var statusExist = await _reqRepository.ForeignKeyExist<RequisitionStatus>(statusId);
        var workTypestatusExist = await _reqRepository.ForeignKeyExist<RequisitionWorkType>(workTypeId);
        var hardwareExist = await _reqRepository.ForeignKeyExist<RequisitionHardware>(hardwareId);
        var locationExist = await _reqRepository.ForeignKeyExist<RequisitionLocation>(locationId);
        var currencyExist = await _reqRepository.ForeignKeyExist<RequisitionCurrency>(currencyId);
        var intervalExist = await _reqRepository.ForeignKeyExist<RequisitionInterval>(intervalId);

        if (!statusExist || !workTypestatusExist || !hardwareExist)
        {
            var errors = new Dictionary<string, string[]>();
            if (!statusExist) errors.Add("StatusId", ["Status not found"]);
            if (!workTypestatusExist) errors.Add("WorkTypeId", ["WorkType not found"]);
            if (!hardwareExist) errors.Add("HardwareId", ["hardware not found"]);
            if (!locationExist) errors.Add("LocationId", ["location not found"]);
            if (!currencyExist) errors.Add("CurrencyId", ["currency not found"]);
            if (!intervalExist) errors.Add("IntervalId", ["interval not found"]);

            return Result.Failure(CommonErrors.InvalidForeignKey(errors));
        }

        return Result.Success();
    }

    public (string Email, string IpAddress, string UserAgent) GetLoggingContext()
    {
        return (
            GetCurrentUserFromToken(),
            GetUserIpAddress(),
            GetUserAgent()
        );
    }
}