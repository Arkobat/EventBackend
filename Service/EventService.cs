using Event.Database;
using Event.Model;
using Event.Model.Exception;
using Event.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Event.Service;

public class EventService
{
    private readonly OrganisationService _orgService;
    private readonly EventDbContext _dbContext;

    public EventService(OrganisationService orgService, EventDbContext dbContext)
    {
        _orgService = orgService;
        _dbContext = dbContext;
    }

    public async Task<List<Model.Event>> GetAll(int memberId, int organisationId)
    {
        await EnsureAccess(memberId, organisationId);
        return await _dbContext.Events
            .Include(e => e.Price)
            .Include(e => e.Teams)
            .ToListAsync();
    }

    public async Task<Model.Event> GetEvent(int memberId, int organisationId, int eventId)
    {
        await EnsureAccess(memberId, organisationId);
        var evt = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (evt == null)
        {
            throw new EventNotFoundException(eventId);
        }

        return evt;
    }

    public async Task<Model.Event> CreateEvent(int memberId, int organisationId, CreateEventTo to)
    {
        // This will ensure access to the organisation
        var organisation = await _orgService.GetOrganisation(memberId, organisationId);

        Price? price = null;
        if (to.Price != null)
        {
            price = new Price
            {
                RegularPrice = to.Price.RegularPrice,
                MemberPrice = to.Price.MemberPrice
            };
        }

        var evt = new Model.Event
        {
            Name = to.Name,
            Price = price,
            Organisation = organisation,
            CreationDate = DateTime.UtcNow,
            StartDate = to.StartDate
        };

        _dbContext.Events.Add(evt);
        await _dbContext.SaveChangesAsync();

        return evt;
    }

    public async Task<Model.Event> UpdateEvent(int memberId, int organisationId, int eventId, CreateEventTo to)
    {
        // This will ensure access to the organisation
        var evt = await GetEvent(memberId, organisationId, eventId);

        evt.Name = to.Name;
        evt.Price = to.Price == null
            ? null
            : new Price {RegularPrice = to.Price.RegularPrice, MemberPrice = to.Price.MemberPrice};
        evt.StartDate = to.StartDate;

        await _dbContext.SaveChangesAsync();
        return evt;
    }

    public async Task DeleteEvent(int memberId, int organisationId, int eventId)
    {
        try
        {
            var evt = await GetEvent(memberId, organisationId, eventId);
            _dbContext.Remove(evt);
        }
        catch (EventNotFoundException)
        {
        }
    }

    /// <summary>
    /// Ensures the member has access to the organisation.
    /// Will throw a NotFoundException if they do not have access
    /// </summary>
    private async Task EnsureAccess(int memberId, int organisationId)
    {
        await _orgService.GetOrganisation(memberId, organisationId);
    }
}