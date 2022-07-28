namespace ExternalSystem.API.Model;

public class ExternalSystemException : Exception
{
    public ExternalSystemError Error { get; protected set; } = new ExternalSystemError();

    public ExternalSystemException(int code, string? message, string? detail, Exception? exception = null) : base(message, exception)
    {
        Error.Code = code;
        Error.Message = message;
        Error.Detail = detail;
    }
}