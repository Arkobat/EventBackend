using Event.Database;
using Event.Model;
using Event.Model.Exception;
using Event.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Event.Service;

public class OrganisationService
{
    private readonly EventDbContext _dbContexts;

    public OrganisationService(EventDbContext dbContexts)
    {
        _dbContexts = dbContexts;
    }

    public async Task<List<Organisation>> GetAll(int memberId)
    {
        return await _dbContexts.Organisations
            .Include(o => o.Members)
            .Include(o => o.Events)
            .Where(o => o.Members.Any(m => m.Id == memberId))
            .ToListAsync();
    }

    public async Task<Organisation> GetOrganisation(int memberId, int organisationId)
    {
        var org = await _dbContexts.Organisations
            .Include(o => o.Members)
            .Include(o => o.Events)
            .Where(o => o.Id == organisationId)
            .FirstOrDefaultAsync();

        // Throw same exception, weather the organisation is null, or if the member does not have access
        if (org == null || !org.IsMember(memberId))
        {
            throw new OrganisationNotFoundException(organisationId);
        }

        return org;
    }

    public async Task<Organisation> CreateOrganisation(int memberId, CreateOrganisationTo to)
    {
        var org = new Organisation
        {
            Name = to.Name
        };

        var user = await _dbContexts.Users.FirstOrDefaultAsync(u => u.Id == memberId);
        if (user == null)
        {
            user = new User
            {
                Username = "username",
                Email = "mail",
                Password = "password",
                Salt = "salt"
            };
            _dbContexts.Users.Add(user);
        }

        org.Members.Add(user);
        await _dbContexts.Organisations.AddAsync(org);
        await _dbContexts.SaveChangesAsync();

        return org;
    }

    public async Task<Organisation> UpdateOrganisation(int memberId, int organisationId, CreateOrganisationTo to)
    {
        var org = await GetOrganisation(memberId, organisationId);
        org.Name = to.Name;
        await _dbContexts.SaveChangesAsync();
        return org;
    }

    public async Task DeleteOrganisation(int memberId, int organisationId)
    {
        try
        {
            var organisation = await GetOrganisation(memberId, organisationId);
            _dbContexts.Remove(organisation);
            await _dbContexts.SaveChangesAsync();
        }
        catch (OrganisationNotFoundException)
        {
        }
    }
}