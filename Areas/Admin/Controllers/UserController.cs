using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Smart_Library.Admin.Models;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;
        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, ApplicationDBContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
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
                    ProfileImage = user.ProfileImage,
                    Address = user.Address,
                    WorkspaceName = user.Workspace?.WorkspaceName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                    CreatedAt = user.CreatedAt,
                    IsLocked = _userManager.IsLockedOutAsync(user).Result
                };
                UserList.Add(UserInfo);
            }
            return View(UserList);
        }
        [HttpGet]
        [Route("Create")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            if (ModelState.IsNullOrEmpty())
            {
                return View();
            }
            // Implement create user logic here
            string imagePaths = UploadImage.UploadSingleImage(model.ProfileImage, "users") ?? "/uploads/images/default-user.png";

            return RedirectToAction("Index", "User");
        }
        // [HttpPost]
        // [Route("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Delete(string id)
        // {
        //     TempData["Message"] = "Xoá người dùng thành công";
        //     TempData["Type"] = "success";
        //     return RedirectToAction("Index", "User");
        // }
    }
}