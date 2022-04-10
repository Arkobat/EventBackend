using System.Security.Claims;
using Event.Model.Exception;
using Event.Model.Request;
using Event.Model.Response;
using Event.Service;
using Microsoft.AspNetCore.Mvc;

namespace Event.Controllers;

[Route("auth")]
public class MemberController : ControllerBase
{
    private readonly AuthService _authService;

    public MemberController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Retrieve an access key for the user
    /// </summary>
    /// <param name="to">The login request</param>
    /// <returns>Authorization credentials</returns>
    [HttpPost("login")]
    public async Task<Credentials> Login([FromBody] LoginTo to)
    {
        return await _authService.Login(to);
    }

    [HttpPost("register")]
    public async Task Register([FromBody] CreateUserTo to)
    {
        await _authService.CreateUser(to);
    }

    /// <summary>
    /// Retrieve a new access token.
    /// This request has to be authorized with the refresh token
    /// </summary>
    /// <returns>Refreshed credentials, including a new refresh token</returns>
    [HttpPost("refresh")]
    public async Task<Credentials> RefreshToken()
    {
        var memberId = GetMemberIdFromRefreshToken();
        return await _authService.RefreshToken(memberId);
    }

    private int GetMemberIdFromRefreshToken()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity)
        {
            throw new ClaimException("Could not find a claim identity");
        }

        var userClaims = identity.Claims;

        var id = userClaims.FirstOrDefault(c => c.Type == "MemberId")?.Value;
        if (id == null)
        {
            throw new ClaimException("Could not find member id");
        }

        return int.Parse(id);
    }
}