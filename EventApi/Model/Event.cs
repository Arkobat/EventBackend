using System.ComponentModel.DataAnnotations;
using Event.Model.Response;
using Event.Service;
using Event.Utils;

namespace Event.Model;

public class Event
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Price? Price { get; set; }
    public List<Team> Teams { get; set; } = new();

    public int OrganisationId { get; set; }
    public Organisation Organisation { get; set; } = null!;

    public DateTime CreationDate { get; set; }
    public DateTime? StartDate { get; set; }

    public EventTo ToTO(IIdResolver idResolver)
    {
        PriceTo? price = null;
        if (Price != null)
        {
            price = new PriceTo
            {
                RegularPrice = Price.RegularPrice,
                MemberPrice = Price.MemberPrice
            };
        }

        var teams = Teams.Select(t => t.ToDto(idResolver)).ToList();
        teams.Sort(); // Sort by points
        return new EventTo
        {
            Id = idResolver.Encrypt(Id),
            Name = Name,
            Price = price,
            Teams = teams
        };
    }
}