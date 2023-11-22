using System.Security.Claims;
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
        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user == null)
            {
                await _next(context);
                return;
            }
            var userRoles = await userManager.GetRolesAsync(user);
            bool rolesChanged = false;
            foreach (var role in userRoles)
            {
                if (!context.User.IsInRole(role))
                {
                    rolesChanged = true;
                    break;
                }
            }
            if (user != null && await userManager.IsLockedOutAsync(user))
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                context.Response.Redirect("/error/locked");
                return;
            }
            if (rolesChanged && user != null)
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
                await signInManager.SignInAsync(user, true);
            }
            await _next(context);
        }
    }

}