using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace tech.haamu.Movie
{
    /// <summary>
    /// An attribute that checks user authentication in HTTP requests.
    /// </summary>
    public class UserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                context.Result = new UnauthorizedResult();
        }
    }
}
