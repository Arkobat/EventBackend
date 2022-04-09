namespace Event.Model.Response;

public class HttpErrorTo
{
    public int Status { get; }
    public string Message { get; set; }

    public HttpErrorTo(int status, string message)
    {
        Status = status;
        Message = message;
    }
}