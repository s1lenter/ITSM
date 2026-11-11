namespace Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (error == Error.None)
        {
            throw new ArgumentException("Failure result cannot contain Error.None.", nameof(error));
        }

        return new(false, error);
    }
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new(true, value, Error.None);
    }

    public new static Result<T> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (error == Error.None)
        {
            throw new ArgumentException("Failure result cannot contain Error.None.", nameof(error));
        }

        return new(false, default, error);
    }
}
