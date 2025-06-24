using System.Security.Claims;

namespace Jat.Entities
{
    public class UserContext
    {
        public ClaimsPrincipal? CurrentUser { get; set; }
    }
}