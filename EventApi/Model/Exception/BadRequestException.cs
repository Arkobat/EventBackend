namespace Event.Model.Exception;

public class BadRequestException : HttpException
{
    public BadRequestException(string message) : base(HttpStatus.BadRequest, message)
    {
    }
}