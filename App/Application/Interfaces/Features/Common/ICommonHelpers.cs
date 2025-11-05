using Application.Common.Results;

namespace Application.Interfaces.Features.Common;

public interface ICommonHelpers
{
    string GetCurrentUserFromToken();

    string GetUserIpAddress();

    string GetUserAgent();

    Task<Result> ValidateForeignKey(int statusId, int workTypeId, int hardwareId, int locationId, int currencyId, int intervalId);

    (string Email, string IpAddress, string UserAgent) GetLoggingContext();
}