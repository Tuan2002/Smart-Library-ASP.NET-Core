using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                await _userManager.DeleteAsync(NewUser);
                ModelState.AddModelError(string.Empty, "Không thể tạo người dùng, lỗi phân quyền");
                return View();
            }
            TempData["UsersMessage"] = "Thêm người dùng thành công";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Users");
        }
        [HttpPost]
        [Route("Lockout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lockout(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                TempData["UsersMessage"] = "Không thể khoá tài khoản của chính mình";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Users");
            }
            var user = await _userManager.FindByIdAsync(id);
            var checkUserLock = await _userManager.IsLockedOutAsync(user);
            if (checkUserLock)
            {
                TempData["UsersMessage"] = "Tài khoản này đã bị khóa từ trước";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            if (user == null)
            {
                TempData["UsersMessage"] = "Người dùng không tồn tại";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            var setLocked = await _userManager.SetLockoutEnabledAsync(user, true);
            var lockUntil = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
            if (!setLocked.Succeeded || !lockUntil.Succeeded)
            {
                TempData["UsersMessage"] = "Không thể khóa tài khoản này";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            TempData["UsersMessage"] = "Đã khóa tài khoản người dùng";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Users");
        }
        [HttpPost]
        [Route("Unlock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["UsersMessage"] = "Người dùng không tồn tại";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            var checkUserLock = await _userManager.IsLockedOutAsync(user);
            if (!checkUserLock)
            {
                TempData["UsersMessage"] = "Tài khoản này không bị khóa";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            var lockUntil = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!lockUntil.Succeeded)
            {
                TempData["UsersMessage"] = "Không thể mở khóa tài khoản này";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            TempData["UsersMessage"] = "Đã mở khóa tài khoản người dùng";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                TempData["UsersMessage"] = "Không thể xoá tài khoản của chính mình";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Users");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["UsersMessage"] = "Người dùng không tồn tại";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            var Result = await _userManager.DeleteAsync(user);
            if (!Result.Succeeded)
            {
                TempData["UsersMessage"] = "Không thể xoá người dùng này";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            TempData["UsersMessage"] = "Xoá người dùng thành công";
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Users");
        }
        [HttpGet]
        [Route("Import")]
        public IActionResult Import(string type)
        {
            switch (type)
            {
                case "excel":
                    return View("ImportExcel");
                case "url":
                    return NotFound();
                default:
                    return NotFound();
            }
        }
        [HttpPost]
        [Route("Import/Excel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(string UserList)
        {
            var ValidUsers = JsonConvert.DeserializeObject<List<ImportUserModel>>(UserList);
            var ImportId = Guid.NewGuid().ToString();
            ImportUsers Import = new ImportUsers(_userManager, _context, _logger);
            var Result = await Import.ImportMultiUserAsync(ValidUsers);
            if (Result.IsNullOrEmpty())
            {
                TempData["UsersMessage"] = "Không có người dùng nào được thêm";
                TempData["Type"] = "warning";
                return RedirectToAction("Index", "Users");
            }
            TempData["ImportId"] = ImportId;
            TempData["Result"] = JsonConvert.SerializeObject(Result).ToString();
            return RedirectToAction("ImportResult", new { importId = ImportId });
        }
        [HttpGet]
        [Route("Import/Result")]
        public IActionResult ImportResult(string? importId)
        {
            var StoredImportId = TempData["ImportId"]?.ToString();
            var JSONData = TempData["Result"]?.ToString();
            if (StoredImportId != importId || StoredImportId == null || importId == null)
                return NotFound();
            var ImportedUsers = JsonConvert.DeserializeObject<List<ImportUserModel>>(JSONData ?? string.Empty);
            if (ImportedUsers.IsNullOrEmpty() || ImportedUsers?.Count == 0)
                return RedirectToAction("Index", "Users");
            var countSucceeded = ImportedUsers?.Count(user => user.Status == "succeeded");
            if (countSucceeded == 0)
            {
                TempData["UsersMessage"] = "Không có người dùng nào được thêm";
                TempData["Type"] = "warning";
            }
            else
            {
                TempData["UsersMessage"] = "Đã thêm " + countSucceeded + " người dùng thành công";
                TempData["Type"] = "success";
            }
            return View(ImportedUsers);
        }
        [HttpGet]
        [Route("Import/Download")]
        public IActionResult DownloadExcelTemplate()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", "DULIEUMAU.xlsx");
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            var file = DownloadFiles.DownloadSingleFile(path);
            if (file == null)
            {
                return NotFound();
            }
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DULIEUMAU.xlsx");
        }

    }
}