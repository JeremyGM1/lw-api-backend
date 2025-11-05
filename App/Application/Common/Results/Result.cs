namespace Application.Common.Results;

public sealed class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    private Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
}

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public Error Error { get; }

    public static Result<T> Success(T data) => new(true, data, Error.None);

    public static Result<T> Failure(Error error) => new(false, default, error);

    private Result(bool isSuccess, T? data, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }
}