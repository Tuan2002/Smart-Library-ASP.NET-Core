
using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;
using Smart_Library.Models;

namespace Smart_Library.Services
{
    public interface IBooksService
    {
        Task<List<CategoryViewModel>> GetCategoriesAsync();
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
        public async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            try
            {
                var categories = await (
                from category in _context.Category
                select new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    CreatedAt = category.CreatedAt,
                    CreatedById = category.CreatedById,
                    CreatedByName = category.CreatedBy.FirstName + " " + category.CreatedBy.LastName,
                    Status = category.Status
                }).ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null!;
            }
        }

    }
}