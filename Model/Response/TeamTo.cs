namespace Event.Model.Response;

public class TeamTo : IComparable<TeamTo>
{
    public string Id { get; init; } = null!;

    /// <summary>
    /// The name of the team
    /// </summary>
    public string Name { get; init; } = null!;
    
    /// <summary>
    /// How many points the team does currently have.
    /// Teams are sorted by points
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// The total amount of participants.
    /// This is not guaranteed to match the amount in participants.
    /// </summary>
    public int ParticipantCount { get; init; }

    /// <summary>
    /// How many of the participant that currently have paid.
    /// This is not guaranteed to match the amount in participants.
    /// </summary>
    public int PaidParticipants { get; init; }

    /// <summary>
    /// What the team has guessed the tie breaker to be
    /// </summary>
    public double TieBreaker { get; set; }

    public int CompareTo(TeamTo? other)
    {
        if (other == null) return 1;

        var pointDiff = other.Points - this.Points;
        if (pointDiff != 0) return pointDiff;

        var tieDiff = other.TieBreaker - this.TieBreaker;
        return tieDiff switch
        {
            0 => 0, // == 0
            > 0 => 1, // > 0
            _ => -1 // < 0
        };
    }
}