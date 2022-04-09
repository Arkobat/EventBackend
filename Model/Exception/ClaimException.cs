namespace Event.Model.Exception;

public class ClaimException: HttpException
{
    public ClaimException(string message) : base(HttpStatus.Unauthorized, message)
    {
    }
}