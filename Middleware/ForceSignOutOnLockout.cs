using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Smart_Library.Entities;

namespace Smart_Library.Middleware
{
    public class ForceSignOutOnLockout
    {
        private readonly RequestDelegate _next;
        public ForceSignOutOnLockout(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && await userManager.IsLockedOutAsync(user))
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                context.Response.Redirect("/error/locked");
                return;
            }
            await _next(context);
        }
    }

}