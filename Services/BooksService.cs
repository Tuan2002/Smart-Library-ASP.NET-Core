using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Services
{
    public interface IBooksService
    {
        Task<ActionResponse> GetCategoriesAsync();
        Task<ActionResponse> GetAuthorsAsync();
        Task<ActionResponse> GetListBookAsync(int? page, int? pageSize);
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
                .Take(10)
                .Select(category => new CategoryViewModel()
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    TotalBooks = category.Books.Count(),
                }).ToListAsync();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Lấy danh mục thành công",
                    Data = categories
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
        public async Task<ActionResponse> GetAuthorsAsync()
        {
            try
            {
                var authors = await _context.Author.AsQueryable()
                .Take(10)
                .Select(author => new AuthorViewModel()
                {
                    AuthorId = author.AuthorId,
                    Name = author.Name,
                    TotalBooks = author.Books.Count(),
                }).ToListAsync();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Lấy danh mục thành công",
                    Data = authors
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
        public async Task<ActionResponse> GetListBookAsync(int? page, int? pageSize)
        {
            try
            {
                var query = _context.Books.AsQueryable();
                var totalBooks = await query.CountAsync();
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var totalPages = (int)Math.Ceiling((double)totalBooks / currentPageSize);
                var books = await query
                    .Where(book => book.IsPublish == true)
                    .Where(book => book.Category.Status == true)
                    .OrderByDescending(book => book.ReaderCount)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .Select(book => new BookViewModel
                    {
                        BookId = book.BookId,
                        Slug = book.Slug,
                        Name = book.Name,
                        ImageURL = book.ImageURL,
                        AuthorId = book.AuthorId,
                        AuthorName = book.Author.Name,
                        AuthorImageURL = book.Author.ImageURL,
                        Pages = book.Pages,
                        ReaderCount = book.ReaderCount ?? 0,
                    }).ToListAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Lấy danh sách sách thành công",
                    Data = new
                    {
                        totalBooks,
                        totalPages,
                        currentPage,
                        currentPageSize,
                        books
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Lấy danh sách sách thất bại"
                };
            }
        }

    }
}