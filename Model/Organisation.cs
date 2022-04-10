using System.ComponentModel.DataAnnotations;
using Event.Model.Response;
using Event.Service;

namespace Event.Model;

public class Organisation
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Event> Events { get; set; } = new();
    public List<User> Members { get; set; } = new();

    public bool IsMember(int id)
    {
        return Members.Any(m => m.Id == id);
    }

    public OrganisationTo ToDto(IIdResolver idResolver)
    {
        return new OrganisationTo
        {
            Id = idResolver.Encrypt(Id),
            Name = Name,
            Events = Events.Select(e => e.ToTO(idResolver)).ToList()
        };
    }
}