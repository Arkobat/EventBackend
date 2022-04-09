using Event.Extension;
using Event.Model.Request;
using Event.Model.Response;
using Event.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers;

[Authorize]
[Route("organisation/{organisationId}/event/{eventId}/team")]
public class TeamController : ControllerBase
{
    private readonly TeamService _service;
    private readonly IIdResolver _idResolver;

    public TeamController(TeamService service, IIdResolver idResolver)
    {
        _service = service;
        _idResolver = idResolver;
    }

    /// <summary>
    /// Create a new team for an event
    /// </summary>
    [HttpPost]
    public async Task<TeamTo> AddTeam(string organisationId, string eventId, [FromBody] CreateTeamTo to)
    {
        var team = await _service.AddTeam(this.GetMemberId(), _idResolver.Decrypt(organisationId),
            _idResolver.Decrypt(eventId), to);
        return team.ToDto(_idResolver);
    }

    /// <summary>
    /// Updates a team
    /// </summary>
    [HttpPut]
    public async Task<TeamTo> UpdateTeam(string organisationId, string eventId, string teamId,
        [FromBody] CreateTeamTo to)
    {
        var team = await _service.UpdateTeam(this.GetMemberId(), _idResolver.Decrypt(teamId), to);
        return team.ToDto(_idResolver);
    }

    /// <summary>
    /// Delete a team
    /// </summary>
    [HttpDelete]
    public async Task DeleteTeam(string organisationId, string eventId, string teamId)
    {
        await _service.DeleteTeam(this.GetMemberId(), _idResolver.Decrypt(teamId));
    }
}