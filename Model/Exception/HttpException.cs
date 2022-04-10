namespace Event.Model.Exception;

public abstract class HttpException : System.Exception
{
    public HttpException(HttpStatus httpStatus, string message)
    {
        HttpStatus = httpStatus;
        Message = message;
    }

    public HttpStatus HttpStatus { get; }
    public string Message { get; }
}