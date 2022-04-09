using System.ComponentModel.DataAnnotations;
using Event.Model.Response;
using Event.Service;

namespace Event.Model;

public class Team
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ParticipantCount { get; set; }
    public int PaidParticipants { get; set; }
    public int Points { get; set; }
    public double TieBreaker { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public TeamTo ToDto(IIdResolver idResolver)
    {
        return new TeamTo
        {
            Id = idResolver.Encrypt(Id),
            Name = Name,
            ParticipantCount = ParticipantCount,
            PaidParticipants = PaidParticipants,

            Points = Points,
            TieBreaker = TieBreaker
        };
    }
}