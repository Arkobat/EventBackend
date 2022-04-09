using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Event.Extension;

public static class ControllerBaseExtension
{
    public static int GetMemberId(this ControllerBase controller)
    {
        if (controller.HttpContext.User.Identity is not ClaimsIdentity identity)
        {
            throw new Exception("Could not find a claim identity");
        }

        var userClaims = identity.Claims;

        var id = userClaims.FirstOrDefault(c => c.Type == "MemberId")?.Value;
        if (id == null)
        {
            throw new Exception("Could not find member id");
        }

        return int.Parse(id);
    }
}