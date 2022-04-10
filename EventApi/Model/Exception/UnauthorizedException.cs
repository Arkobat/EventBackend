namespace Event.Model.Exception;

public class UnauthorizedException : HttpException
{
    public UnauthorizedException(string message) : base(HttpStatus.Unauthorized, message)
    {
    }
}