namespace Application.Interfaces;
public interface ICommonHelpers
{
    string GetCurrentUserFromToken();
    void ValidateEntityExist<T>(T entity, string id);
    void ValidateForeighKey(int statusId, int workTypeId, int hardwareId);
}