using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Quản trị viên")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;
        public UsersController(ILogger<UsersController> logger, UserManager<ApplicationUser> userManager, ApplicationDBContext context)
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
            var IsEmailUsed = await _userManager.FindByEmailAsync(model.Email);
            if (IsEmailUsed != null)
            {
                ModelState.AddModelError(string.Empty, "Email này đã được sử dụng cho một tài khoản khác");
                return View(model);
            }
            var NewUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.GetAddress(),
                DateOfBirth = model.DateOfBirth,
                ProfileImage = UploadImage.UploadSingleImage(model.ProfileImage) ?? "/uploads/images/default-user.png",
                CreatedAt = DateTime.Now,
                WorkspaceId = model.WorkspaceId,
            };
            var CreateNewUser = await _userManager.CreateAsync(NewUser, model.GeneratePassword());
            if (!CreateNewUser.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Không thể tạo người dùng, vui lòng thử lại sau");
                return View();
            }
            var AddRole = await _userManager.AddToRoleAsync(NewUser, model.RoleName);
            if (!AddRole.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Không thể tạo người dùng, lỗi phân quyền");
                return View();
            }
            TempData["Message"] = "Thêm người dùng thành công";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Users");
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