
using Jat.Entities;

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
            userContext.CurrentUser = context.User;
            await _next(context);
        }
    }
}