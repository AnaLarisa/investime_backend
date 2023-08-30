using System.Security.Claims;

namespace InvesTime.BackEnd.Helpers;

public class UserHelper : IUserHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string GetCurrentUserId()
    {
        var httpContext = _contextAccessor.HttpContext;
        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               throw new InvalidOperationException("Current user ID not found.");
        ;
    }

    public string GetCurrentUserUsername()
    {
        var httpContext = _contextAccessor.HttpContext;
        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value ??
               throw new InvalidOperationException("Current user username not found.");
    }

    public bool IsCurrentUserAdmin()
    {
        var httpContext = _contextAccessor.HttpContext;
        var claimsIdentity = httpContext?.User.Identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value == "Admin";
    }
}