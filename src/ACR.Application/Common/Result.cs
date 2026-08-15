namespace ACR.Application.Common;

public sealed record Error(string Code, string Message);

public sealed class Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(T? value, Error? error)
    {
        _value = value;
        _error = error;
    }

    public static Result<T> Success(T value) => new (value, null);

    public static Result<T> Fail(Error error) => new (default, error);

    public static Result<T> Fail(string code, string message) => Fail(new Error(code, message));

    public bool HasError => _error is not null;

    public T Value => _value;
    
    public Error Error => _error;
}