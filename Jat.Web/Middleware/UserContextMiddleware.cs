
using Jat.Entities;
using System.Security.Claims;

namespace Jat.Web.Middleware
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserContext userContext)
        {
            // CurrentUser?.Identity?.Name
            var defaultUser = new ClaimsIdentity();
            defaultUser.AddClaim(new Claim(ClaimTypes.Name, "System"));
            userContext.CurrentUser = new ClaimsPrincipal([defaultUser]);
            
            await _next(context);
        }
    }
}