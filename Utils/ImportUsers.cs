using Microsoft.AspNetCore.Identity;
using Smart_Library.Admin.Models;
using Smart_Library.Areas.Admin.Controllers;
using Smart_Library.Data;
using Smart_Library.Entities;

namespace Smart_Library.Utils
{
    public class ImportUsers
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDBContext _context;
        public ImportUsers(UserManager<ApplicationUser> userManager, ApplicationDBContext context, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        public async Task<List<ImportUserModel>?> ImportMultiUserAsync(List<ImportUserModel>? users)
        {
            var Result = new List<ImportUserModel>();
            if (users?.Count < 0 || users == null)
                return null;
            foreach (var user in users)
                try
                {
                    var IsEmailUsed = await _userManager.FindByEmailAsync(user.Email);
                    if (IsEmailUsed != null)
                    {
                        user.Status = "failed";
                        user.Message = "Email đã được sử dụng";
                        Result.Add(user);
                        continue;
                    }
                    var NewUser = new ApplicationUser
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.Phone,
                        DateOfBirth = user.DateOfBirth,
                        Address = user.GetAddress(),
                        CreatedAt = DateTime.Now,
                        ProfileImage = "/img/default-user.webp",
                        WorkspaceId = user.WorkspaceId
                    };
                    var ResultCreate = await _userManager.CreateAsync(NewUser, user.GeneratePassword());
                    if (!ResultCreate.Succeeded)
                    {
                        user.Status = "failed";
                        user.Message = "Thông tin không hợp lệ";
                        Result.Add(user);
                        continue;
                    }
                    var ResultAddRole = await _userManager.AddToRoleAsync(NewUser, user.RoleName);
                    if (!ResultAddRole.Succeeded)
                    {
                        await _userManager.DeleteAsync(NewUser);
                        user.Status = "failed";
                        user.Message = "Không thể phân quyền";
                        Result.Add(user);
                        continue;
                    }
                    user.Status = "succeeded";
                    user.Message = "Đã thêm người dùng";
                    Result.Add(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    user.Status = "failed";
                    user.Message = "Lỗi hệ thống";
                    Result.Add(user);
                }
            return Result;
        }
    }
}