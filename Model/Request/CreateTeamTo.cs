using System.ComponentModel.DataAnnotations;

namespace Event.Model.Request;

public class CreateTeamTo
{
    [Required]
    public string Name { get; set; } = null!;
    
    public int ParticipantCount { get; set; }
    public int PaidParticipants { get; set; }
    
    public int Points { get; set; }
    public double TieBreaker { get; set; }
}