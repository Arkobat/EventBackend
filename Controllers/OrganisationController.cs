using Event.Extension;
using Event.Model.Request;
using Event.Model.Response;
using Event.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers;

[Authorize]
[Route("organisation")]
public class OrganisationController : ControllerBase
{
    private readonly OrganisationService _service;
    private readonly IIdResolver _idResolver;

    public OrganisationController(OrganisationService service, IIdResolver idResolver)
    {
        _service = service;
        _idResolver = idResolver;
    }

    /// <summary>
    /// Get all organisations the user have access to
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<OrganisationTo>> GetAll()
    {
        var orgs = await _service.GetAll(this.GetMemberId());
        return orgs.Select(o => o.ToDto(_idResolver)).ToList();
    }

    /// <summary>
    /// Get an organisation
    /// </summary>
    [HttpGet("{organisationId}")]
    public async Task<OrganisationTo> Get(string organisationId)
    {
        var org = await _service.GetOrganisation(this.GetMemberId(), _idResolver.Decrypt(organisationId));
        return org.ToDto(_idResolver);
    }

    /// <summary>
    /// Create an organisation
    /// </summary>
    [HttpPost]
    public async Task<OrganisationTo> Create([FromBody] CreateOrganisationTo to)
    {
        var org = await _service.CreateOrganisation(this.GetMemberId(), to);
        return org.ToDto(_idResolver);
    }

    /// <summary>
    /// Update an organisation
    /// </summary>
    [HttpPut("{organisationId}")]
    public async Task<OrganisationTo> Update(string organisationId, [FromBody] CreateOrganisationTo to)
    {
        var org = await _service.UpdateOrganisation(this.GetMemberId(), _idResolver.Decrypt(organisationId), to);
        return org.ToDto(_idResolver);
    }

    /// <summary>
    /// Delete an organisation
    /// </summary>
    [HttpDelete("{organisationId}")]
    public async Task Delete(string organisationId)
    {
        await _service.DeleteOrganisation(this.GetMemberId(), _idResolver.Decrypt(organisationId));
    }

    [HttpPost("{organisationId}/members")]
    public async Task AddMember(string organisationId, [FromBody] OrganisationMemberRequest username)
    {
        await _service.AddMember(this.GetMemberId(), _idResolver.Decrypt(organisationId), username);
    }

    /// <summary>
    /// Delete an organisation
    /// </summary>
    [HttpDelete("{organisationId}/members")]
    public async Task RemoveMember(string organisationId, [FromBody] OrganisationMemberRequest username)
    {
        await _service.RemoveMember(this.GetMemberId(), _idResolver.Decrypt(organisationId), username);
    }
}