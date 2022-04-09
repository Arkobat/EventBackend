namespace Event.Model.Exception;

public class NotFoundException : HttpException
{
    public NotFoundException(string message) : base(HttpStatus.NotFound, message)
    {
    }
}