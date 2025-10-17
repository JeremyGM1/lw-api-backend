using FluentValidation.Results;

namespace Application.Common.Helpers;

public interface ICommonHelpers
{
    string GetCurrentUserFromToken();

    void ThrowIfValidationFails(ValidationResult validationResult);

    //T ValidateIfEntityExist<T>(T? entity, string id) where T : class;

    void ValidateForeignKey(int statusId, int workTypeId, int hardwareId);
}