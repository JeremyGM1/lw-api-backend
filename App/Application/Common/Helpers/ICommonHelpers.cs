using FluentValidation.Results;

namespace Application.Common.Helpers;

public interface ICommonHelpers
{
    string GetCurrentUserFromToken();

    void ThrowIfValidationFails(ValidationResult validationResult);

    void ValidateIfEntityExist<T>(T entity, string id);

    void ValidateForeignKey(int statusId, int workTypeId, int hardwareId);
}