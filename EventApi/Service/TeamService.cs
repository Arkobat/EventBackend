using Event.Database;
using Event.Model;
using Event.Model.Exception;
using Event.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Event.Service;

public class TeamService
{
    private readonly EventDbContext _dbContext;
    private readonly EventService _eventService;

    public TeamService(EventDbContext dbContext, EventService eventService)
    {
        _dbContext = dbContext;
        _eventService = eventService;
    }


    public async Task<Team> AddTeam(int memberId, int organisationId, int eventId, CreateTeamTo to)
    {
        var evt = await _eventService.GetEvent(memberId, organisationId, eventId);
        var team = new Team
        {
            Name = to.Name,
            ParticipantCount = to.ParticipantCount,
            PaidParticipants = to.PaidParticipants,
        };
        _dbContext.Teams.Add(team);
        evt.Teams.Add(team);
        await _dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<Team> UpdateTeam(int memberId, int teamId, CreateTeamTo to)
    {
        var team = await GetTeamAndEnsureAccess(memberId, teamId);

        team.Name = to.Name;
        team.ParticipantCount = to.ParticipantCount;
        team.PaidParticipants = to.PaidParticipants;
        team.Points = to.Points;
        team.TieBreaker = to.TieBreaker;

        await _dbContext.SaveChangesAsync();
        return team;
    }

    public async Task DeleteTeam(int memberId, int teamId)
    {
        try
        {
            var team = await GetTeamAndEnsureAccess(memberId, teamId);
            _dbContext.Remove(team);
            await _dbContext.SaveChangesAsync();
        }
        catch (TeamNotFoundException)
        {
        }
    }

    private async Task<Team> GetTeamAndEnsureAccess(int memberId, int teamId)
    {
        var team = await _dbContext.Teams
            .Include(t => t.Event)
            .Where(t => t.Id == teamId)
            .FirstOrDefaultAsync();

        if (team == null)
        {
            throw new TeamNotFoundException(teamId);
        }

        try
        {
            var evt = team.Event;
            // Ensure user have access to the organisation
            await _eventService.GetEvent(memberId, evt.OrganisationId, evt.Id);
        }
        catch (NotFoundException)
        {
            // Throw same error, weather the not found, or not access
            throw new TeamNotFoundException(teamId);
        }

        return team;
    }
}