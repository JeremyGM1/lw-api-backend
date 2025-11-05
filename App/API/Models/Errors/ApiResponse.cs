using Application.Common.Results;

namespace API.Models.Errors;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public Error Error { get; set; }

    public static ApiResponse<T> SuccessResponse(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ApiResponse<T> SuccessResponse() => new()
    {
        IsSuccess = true
    };

    public static ApiResponse<T> FailureResponse(Error error) => new()
    {
        IsSuccess = false,
        Error = error
    };

    public static ApiResponse<T> FromResult(Result result) => new()
    {
        IsSuccess = result.IsSuccess,
        Error = result.Error
    };

    public static ApiResponse<T> FromResult(Result<T> result) => new()
    {
        IsSuccess = result.IsSuccess,
        Data = result.Data,
        Error = result.Error
    };
}