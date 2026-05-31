namespace SMT.API.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }
    public DateTime Timestamp { get; init; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        IsSuccess = true,
        Data = data,
        Timestamp = DateTime.UtcNow
    };

    public static ApiResponse<T> Fail(string code, string message) => new()
    {
        IsSuccess = false,
        Error = new ApiError(code, message),
        Timestamp = DateTime.UtcNow
    };
}

public class ApiError
{
    public string Code { get; init; }
    public string Message { get; init; }

    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}