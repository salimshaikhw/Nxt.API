using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nxt.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserEmail(this HttpContext httpContext)
        {
            return httpContext.User.FindFirst(JwtRegisteredClaimNames.Email).Value;
        }
    }
}
