using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IBooksManagerService
    {
        Task<ActionMessage> CreateCategoryAsync(CreateCategoryModel category);
        Task<ActionMessage> HideCategoryAsync(string id);
        Task<ActionMessage> ShowCategoryAsync(string id);
        Task<ActionMessage> UpdateCategoryAsync(string id, string name);
        Task<ActionMessage> DeleteCategoryAsync(string id);
    }
    public class BooksManagerService : IBooksManagerService
    {
        public readonly ILogger<BooksManagerService> _logger;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _context;

        public BooksManagerService(ILogger<BooksManagerService> logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ApplicationDBContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
        }
        public async Task<ActionMessage> CreateCategoryAsync(CreateCategoryModel category)
        {
            var result = new ActionMessage();
            try
            {
                string UserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(UserId))
                {
                    result.IsSuccess = false;
                    result.Message = "Không tìm thấy người dùng để thêm danh mục";
                    return result;
                }
                var newCategory = new Category()
                {
                    Name = category.Name,
                    CreatedAt = DateTime.Now,
                    CreatedById = UserId,
                    Status = category.Status
                };
                await _context.Category.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = "Thêm danh mục thành công";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsSuccess = false;
                result.Message = "Không thể thêm danh mục";
            }
            return result;
        }
        public async Task<ActionMessage> HideCategoryAsync(string id)
        {
            var result = new ActionMessage();
            try
            {
                int categoryId = Convert.ToInt32(id);
                var category = await _context.Category.FindAsync(categoryId);
                if (category == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Không tìm thấy danh mục";
                    return result;
                }
                category.Status = false;
                _context.Category.Update(category);
                _context.SaveChanges();
                result.IsSuccess = true;
                result.Message = "Ẩn danh mục thành công";
            }
            catch (System.Exception)
            {
                result.IsSuccess = false;
                result.Message = "Không thể ẩn danh mục";
            }
            return result;
        }
        public async Task<ActionMessage> ShowCategoryAsync(string id)
        {
            var result = new ActionMessage();
            try
            {
                int categoryId = Convert.ToInt32(id);
                var category = await _context.Category.FindAsync(categoryId);
                if (category == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Không tìm thấy danh mục";
                    return result;
                }
                category.Status = true;
                _context.Category.Update(category);
                _context.SaveChanges();
                result.IsSuccess = true;
                result.Message = "Đã hiển thị danh mục thành công";
            }
            catch (System.Exception)
            {
                result.IsSuccess = false;
                result.Message = "Không thể hiện danh mục";
            }
            return result;
        }
        public async Task<ActionMessage> DeleteCategoryAsync(string id)
        {
            var result = new ActionMessage();
            try
            {
                int categoryId = Convert.ToInt32(id);
                var category = await _context.Category.FindAsync(categoryId);
                if (category == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Không tìm thấy danh mục";
                    return result;
                }
                _context.Category.Remove(category);
                _context.SaveChanges();
                result.IsSuccess = true;
                result.Message = "Xóa danh mục thành công";
            }
            catch (System.Exception)
            {
                result.IsSuccess = false;
                result.Message = "Không thể xóa danh mục";
            }
            return result;
        }
        public async Task<ActionMessage> UpdateCategoryAsync(string id, string name)
        {
            var result = new ActionMessage();
            try
            {
                int categoryId = Convert.ToInt32(id);
                if (string.IsNullOrEmpty(name))
                {
                    result.IsSuccess = false;
                    result.Message = "Tên danh mục không hợp lệ";
                    return result;
                }
                var category = await _context.Category.FindAsync(categoryId);
                if (category == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Không tìm thấy danh mục";
                    return result;
                }
                category.Name = name;
                _context.Category.Update(category);
                _context.SaveChanges();
                result.IsSuccess = true;
                result.Message = "Cập nhật danh mục thành công";
            }
            catch (System.Exception)
            {
                result.IsSuccess = false;
                result.Message = "Không thể cập nhật danh mục";
            }
            return result;
        }
    }
}