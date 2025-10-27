using FluentValidation.Results;

namespace Application.Common.Helpers;

public interface ICommonHelpers
{
    string GetCurrentUserFromToken();

    void ThrowIfValidationFails(ValidationResult validationResult);

    string GetUserIpAddress();

    string GetUserAgent();

    void ValidateForeignKey(int statusId, int workTypeId, int hardwareId);

    (string Email, string IpAddress, string UserAgent) GetLoggingContext();
}