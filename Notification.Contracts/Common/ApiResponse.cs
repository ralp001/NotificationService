namespace Notification.Contracts.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; }
    public string TraceId { get; set; }

    public ApiResponse(T data, string message = "Success")
    {
        Success = true;
        Message = message;
        Data = data;
        Errors = new List<string>();
        TraceId = Guid.NewGuid().ToString();
    }

    public static ApiResponse<T> Failure(List<string> errors, string message = "Failed")
    {
        return new ApiResponse<T>(default!)
        {
            Success = false,
            Message = message,
            Errors = errors,
            TraceId = Guid.NewGuid().ToString()
        };
    }
}