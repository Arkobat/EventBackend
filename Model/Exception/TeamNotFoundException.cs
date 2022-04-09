namespace Event.Model.Exception;

public class TeamNotFoundException : NotFoundException
{
    public TeamNotFoundException(int id) : base($"Could not find team with id {id}")
    {
    }
}