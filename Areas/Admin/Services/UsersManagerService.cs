using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IUsersManagerService
    {
        Task<ActionResponse> GetUsersListAsync(int? page, int? pageSize);
        Task<ActionResponse> CreateUserAsync(CreateUserModel user);
        Task<ActionResponse> UpdateUserAsync(string id, EditUserModel user);
        Task<UserViewModel> GetUserByIdAsync(string userId);
        Task<ActionResponse> LockoutUserAsync(string userId);
        Task<ActionResponse> UnlockUserAsync(string userId);
        Task<ActionResponse> DeleteUserAsync(string userId);
        Task<List<ImportUserModel>?> ImportMultiUserAsync(List<ImportUserModel>? users);
    }
    public class UsersManagerService : IUsersManagerService
    {
        private readonly ILogger<UsersManagerService> _logger;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersManagerService(UserManager<ApplicationUser> userManager, ApplicationDBContext context, IHttpContextAccessor httpContext, ILogger<UsersManagerService> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContext;
        }
        public async Task<ActionResponse> GetUsersListAsync(int? page, int? pageSize)
        {
            try
            {
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var query = _userManager.Users.AsQueryable();
                var totalUsers = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalUsers / currentPageSize);
                var result = await query.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize).ToListAsync();
                var users = result.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileImage = user.ProfileImage,
                    Address = user.Address,
                    Phone = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    WorkspaceId = user.WorkspaceId,
                    WorkspaceName = user.Workspace.WorkspaceName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                    CreatedAt = user.CreatedAt,
                    IsLocked = _userManager.IsLockedOutAsync(user).Result
                }).ToList();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Lấy danh sách người dùng thành công",
                    Data = new
                    {
                        users,
                        totalPages,
                        currentPage,
                        currentPageSize,
                        totalUsers
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể lấy danh sách người dùng"
                };
            }

        }
        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                    throw new Exception("Người dùng không tồn tại");
                var UserInfo = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileImage = user.ProfileImage,
                    Address = user.Address,
                    Phone = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    WorkspaceId = user.WorkspaceId,
                    WorkspaceName = user.Workspace?.WorkspaceName,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                    CreatedAt = user.CreatedAt,
                    IsLocked = _userManager.IsLockedOutAsync(user).Result
                };
                return UserInfo;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null!;
            }
        }
        public async Task<ActionResponse> UpdateUserAsync(string id, EditUserModel user)
        {
            try
            {
                var CurrentUser = await _userManager.FindByIdAsync(id);
                if (CurrentUser is null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Người dùng không tồn tại"
                    };
                }
                CurrentUser.Email = user.Email ?? CurrentUser.Email;
                CurrentUser.FirstName = user.FirstName ?? CurrentUser.FirstName;
                CurrentUser.LastName = user.LastName ?? CurrentUser.LastName;
                CurrentUser.Address = user.Address ?? CurrentUser.Address;
                CurrentUser.PhoneNumber = user.Phone ?? CurrentUser.PhoneNumber;
                CurrentUser.DateOfBirth = user.DateOfBirth ?? CurrentUser.DateOfBirth;
                CurrentUser.WorkspaceId = user.WorkspaceId ?? CurrentUser.WorkspaceId;
                if (user.ImageFile != null)
                {
                    CurrentUser.ProfileImage = UploadImage.UploadSingleImage(user.ImageFile) ?? CurrentUser.ProfileImage;
                }
                var CurrentRole = await _userManager.GetRolesAsync(CurrentUser);
                if (!CurrentRole.Contains(user.RoleName) && user.RoleName != null)
                {
                    var RemoveRole = await _userManager.RemoveFromRolesAsync(CurrentUser, CurrentRole);
                    var AddRole = await _userManager.AddToRoleAsync(CurrentUser, user.RoleName);
                    if (!AddRole.Succeeded)
                    {
                        return new ActionResponse
                        {
                            IsSuccess = false,
                            Message = "Không thể cập nhật thông tin người dùng"
                        };
                    }
                }
                var Result = await _userManager.UpdateAsync(CurrentUser);
                if (!Result.Succeeded)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không thể cập nhật thông tin người dùng"
                    };
                }
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật thông tin người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể cập nhật thông tin người dùng"
                };
            }
        }
        public async Task<ActionResponse> CreateUserAsync(CreateUserModel user)
        {
            var IsEmailUsed = await _userManager.FindByEmailAsync(user.Email);
            if (IsEmailUsed != null)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Email đã được sử dụng"
                };
            }
            var NewUser = new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.GetAddress(),
                DateOfBirth = user.DateOfBirth,
                ProfileImage = UploadImage.UploadSingleImage(user.ProfileImage) ?? "/img/default-user.webp",
                CreatedAt = DateTime.UtcNow,
                WorkspaceId = user.WorkspaceId,
            };
            var CreateNewUser = await _userManager.CreateAsync(NewUser, user.GeneratePassword());
            if (!CreateNewUser.Succeeded)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể tạo người dùng, vui lòng thử lại sau!"
                };
            }
            var AddRole = await _userManager.AddToRoleAsync(NewUser, user.RoleName);
            if (!AddRole.Succeeded)
            {
                await _userManager.DeleteAsync(NewUser);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể tạo người dùng, lỗi phân quyền!"
                };
            }
            return new ActionResponse
            {
                IsSuccess = true,
                Message = "Tạo người dùng thành công!"
            };
        }
        public async Task<ActionResponse> LockoutUserAsync(string userId)
        {
            if (userId == _userManager.GetUserId(_httpContextAccessor?.HttpContext?.User))
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể khóa tài khoản của chính mình"
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            var checkUserLock = await _userManager.IsLockedOutAsync(user);
            if (user == null)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Người dùng không tồn tại"
                };
            }
            if (checkUserLock)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Người dùng này đã bị khóa từ trước"
                };
            }
            var setLocked = await _userManager.SetLockoutEnabledAsync(user, true);
            var lockUntil = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
            if (!setLocked.Succeeded || !lockUntil.Succeeded)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể khóa tài khoản người dùng"
                };
            }
            return new ActionResponse
            {
                IsSuccess = true,
                Message = "Khóa tài khoản người dùng thành công"
            };
        }
        public async Task<ActionResponse> UnlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Người dùng không tồn tại"
                };
            }
            var checkUserLock = await _userManager.IsLockedOutAsync(user);
            if (!checkUserLock)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Tài khoản này không bị khóa"
                };
            }
            var lockUntil = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!lockUntil.Succeeded)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể mở khóa tài khoản này"
                };
            }
            return new ActionResponse
            {
                IsSuccess = true,
                Message = "Đã mở khóa tài khoản người dùng"
            };
        }
        public async Task<ActionResponse> DeleteUserAsync(string userId)
        {
            if (userId == _userManager.GetUserId(_httpContextAccessor?.HttpContext?.User))
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể xoá tài khoản của chính mình"
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Người dùng không tồn tại"
                };
            }
            var Result = await _userManager.DeleteAsync(user);
            if (!Result.Succeeded)
            {
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể xoá người dùng này"
                };
            }
            return new ActionResponse
            {
                IsSuccess = true,
                Message = "Đã xoá người dùng thành công"
            };
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
                        CreatedAt = DateTime.UtcNow,
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