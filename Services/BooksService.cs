
using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Services
{
    public interface IBooksService
    {
        Task<ActionResponse> GetCategoriesAsync();
    }
    internal class ActionStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; } = null!;
    }
    public class BooksService : IBooksService
    {
        public readonly ApplicationDBContext _context;
        public readonly ILogger<BooksService> _logger;
        public BooksService(ApplicationDBContext context, ILogger<BooksService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ActionResponse> GetCategoriesAsync()
        {
            try
            {
                var categories = await _context.Category.AsQueryable()
                .Where(category => category.Status == true)
                .Select(category => new CategoryViewModel()
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    CreatedAt = category.CreatedAt,
                    CreatedByName = category.CreatedBy.FirstName + " " + category.CreatedBy.FirstName,
                    Status = category.Status
                }).ToListAsync();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Lấy danh mục thành công",
                    Data = categories as List<CategoryViewModel>
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Không thể lấy danh mục"
                };
            }
        }

    }
}