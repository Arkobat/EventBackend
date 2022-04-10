namespace Event.Model.Response;

public class OrganisationTo
{
    public string Id { get; init; } = null!;

    /// <summary>
    /// The name of the organisation
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// A list of all the events in the organisation
    /// </summary>
    public List<EventTo> Events { get; set; } = new();
}