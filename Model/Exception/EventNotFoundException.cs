namespace Event.Model.Exception;

public class EventNotFoundException : NotFoundException
{
    public EventNotFoundException(int id) : base($"Could not find event with id {id}")
    {
    }
}