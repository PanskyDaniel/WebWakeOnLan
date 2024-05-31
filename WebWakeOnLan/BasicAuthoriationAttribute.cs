using Microsoft.AspNetCore.Authorization;

namespace WebWakeOnLan
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationSchemes;
        }
    }
}
