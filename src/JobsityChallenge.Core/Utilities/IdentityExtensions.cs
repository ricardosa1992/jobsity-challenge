using System.Security.Claims;

namespace JobsityChallenge.Core.Utilities;

public static class IdentityExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
