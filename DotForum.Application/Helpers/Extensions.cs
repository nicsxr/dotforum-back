using System.Security.Claims;

namespace DotForum.Application.Helpers;

public static class Extensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}