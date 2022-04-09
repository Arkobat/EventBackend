namespace Event.Model.Exception;

public class OrganisationNotFoundException : NotFoundException
{
    public OrganisationNotFoundException(int id) : base($"Could not find any organisation with the id {id}")
    {
    }
}