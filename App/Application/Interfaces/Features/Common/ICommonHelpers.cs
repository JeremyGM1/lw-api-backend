using FluentValidation.Results;

namespace Application.Interfaces.Features.Common;

public interface ICommonHelpers
{
    string GetCurrentUserFromToken();

    void ThrowIfValidationFails(ValidationResult validationResult);

    string GetUserIpAddress();

    string GetUserAgent();

    void ValidateForeignKey(int statusId, int workTypeId, int hardwareId, int locationId, int currencyId, int intervalId);

    (string Email, string IpAddress, string UserAgent) GetLoggingContext();
}