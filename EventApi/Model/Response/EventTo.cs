namespace Event.Model.Response;

public class EventTo
{
    public string Id { get; set; } = null!;

    /// <summary>
    /// The name of the event
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The price for the event.
    /// If the event is free, this field is null
    /// </summary>
    public PriceTo? Price { get; init; }

    /// <summary>
    /// Returns weather or not the event has a price
    /// </summary>
    public bool HasPrice => Price != null;

    /// <summary>
    /// The teams for the event
    /// </summary>
    public List<TeamTo> Teams { get; set; } = new();

    public DateTime CreationDate { get; set; }
    public DateTime? StartDate { get; set; }
}