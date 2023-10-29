using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Entities;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var UserList = new List<UserViewModel>();
            foreach (var user in users)
            {
                var UserInfo = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                    CreatedAt = user.CreatedAt,
                    Status = _userManager.IsLockedOutAsync(user).Result
                };
                UserList.Add(UserInfo);
            }
            return View(UserList);
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            TempData["Message"] = "Xoá người dùng thành công";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "User");
        }
    }
}