using Event.Model.Request;
using Event.Model.Response;
using Event.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Event.Extension;
using Event.Utils;

namespace Event.Controllers;

[Authorize]
[Route("organisation/{organisationId}/event")]
public class EventController : ControllerBase
{
    private readonly EventService _service;
    private readonly IIdResolver _idResolver;

    public EventController(EventService service, IIdResolver idResolver)
    {
        _service = service;
        _idResolver = idResolver;
    }

    /// <summary>
    /// Get all events for an organisation
    /// </summary>
    [HttpGet]
    public async Task<List<EventTo>> GetAll([FromRoute] string organisationId)
    {
        var events = await _service.GetAll(this.GetMemberId(), _idResolver.Decrypt(organisationId));
        return events.Select(e => e.ToTO(_idResolver)).ToList();
    }

    /// <summary>
    /// Get a single event
    /// </summary>
    [HttpGet("{eventId}")]
    public async Task<EventTo> Get([FromRoute] string organisationId, string eventId)
    {
        var evt = await _service.GetEvent(this.GetMemberId(), _idResolver.Decrypt(organisationId),
            _idResolver.Decrypt(eventId));
        return evt.ToTO(_idResolver);
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    [HttpPost]
    public async Task<EventTo> Create([FromRoute] string organisationId, [FromBody] CreateEventTo to)
    {
        var evt = await _service.CreateEvent(this.GetMemberId(), _idResolver.Decrypt(organisationId), to);
        return evt.ToTO(_idResolver);
    }

    /// <summary>
    /// Update an event
    /// </summary>
    [HttpPut("{eventId}")]
    public async Task<EventTo> Update([FromRoute] string organisationId, string eventId, [FromBody] CreateEventTo to)
    {
        var evt = await _service.UpdateEvent(this.GetMemberId(), _idResolver.Decrypt(organisationId),
            _idResolver.Decrypt(eventId), to);
        return evt.ToTO(_idResolver);
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    [HttpDelete("{eventId}")]
    public async Task Delete([FromRoute] string organisationId, string eventId)
    {
        await _service.DeleteEvent(this.GetMemberId(), _idResolver.Decrypt(organisationId),
            _idResolver.Decrypt(eventId));
    }
}