using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "RoleOptions")]
    public class RoleList : ViewComponent
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleList(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return await Task.FromResult((IViewComponentResult)View("RoleOptions", roles));
        }

    }
}