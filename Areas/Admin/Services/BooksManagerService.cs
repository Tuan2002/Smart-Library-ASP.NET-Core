using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Slugify;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IBooksManagerService
    {
        Task<ActionResponse> GetBooksAsync(int? page, int? pageSize);
        Task<ActionResponse> GetBookByIdAsync(int id);
        Task<ActionResponse> CreateBookAsync(CreateBookModel book);
        Task<ActionResponse> UpdateBookAsync(int id, UpdateBookModel book);
        Task<ActionResponse> ShowBookAsync(int id);
        Task<ActionResponse> HideBookAsync(int id);
        Task<ActionResponse> DeleteBookAsync(int id);
    }

    public class BooksManagerService : IBooksManagerService
    {
        public readonly ILogger<BooksManagerService> _logger;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDBContext _context;

        public BooksManagerService(ILogger<BooksManagerService> logger, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<ActionResponse> GetBooksAsync(int? page, int? pageSize)
        {
            try
            {
                var query = _context.Books.AsQueryable();
                var totalBooks = await query.CountAsync();
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var totalPages = (int)Math.Ceiling((double)totalBooks / currentPageSize);
                var books = await query
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .Select(book => new BookViewModel
                    {
                        BookId = book.BookId,
                        Name = book.Name,
                        ImageURL = book.ImageURL,
                        AuthorId = book.AuthorId,
                        AuthorName = book.Author.Name,
                        PublisherId = book.PublisherId,
                        PublisherName = book.Publisher.Name,
                        CategoryId = book.CategoryId,
                        CategoryName = book.Category.Name,
                        Pages = book.Pages,
                        Language = book.Language,
                        AddedAt = book.AddedAt,
                        AddedById = book.AddedById,
                        AddedByName = book.AddedBy.FirstName + " " + book.AddedBy.LastName,
                        IsPublish = book.IsPublish
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

        public async Task<ActionResponse> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy sách"
                    };
                }

                var bookViewModel = new BookViewModel
                {
                    BookId = book.BookId,
                    Name = book.Name,
                    ShortDescription = book.ShortDescription,
                    Description = book.Description,
                    ImageURL = book.ImageURL,
                    AuthorId = book.AuthorId,
                    AuthorName = book.Author.Name,
                    PublisherId = book.PublisherId,
                    PublisherName = book.Publisher.Name,
                    CategoryId = book.CategoryId,
                    CategoryName = book.Category.Name,
                    Pages = book.Pages,
                    Language = book.Language,
                    IsPublish = book.IsPublish,
                };
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Lấy thông tin sách thành công",
                    Data = bookViewModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Lấy thông tin sách thất bại"
                };
            }
        }

        public async Task<ActionResponse> CreateBookAsync(CreateBookModel book)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Bạn cần đăng nhập để thực hiện chức năng này"
                    };
                }

                var newBook = new Book
                {
                    Name = book.Name,
                    ShortDescription = book.ShortDescription,
                    Description = book.Description,
                    Slug = new SlugHelper().GenerateSlug(book.Name),
                    ImageURL = UploadImage.UploadSingleImage(book.Image) ?? "/uploads/images/default-book.png",
                    IsEBook = book.IsEBook ?? false,
                    Language = book.Language,
                    Pages = book.Pages,
                    ReaderCount = 0,
                    CategoryId = book.CategoryId,
                    AuthorId = book.AuthorId,
                    PublisherId = book.PublisherId,
                    AddedById = userId,
                    IsPublish = book.IsPublish,
                    AddedAt = DateTime.UtcNow,
                };
                var result = await _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Thêm sách mới thành công",
                    Data = result.Entity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Thêm sách thất bại"
                };
            }
        }

        public async Task<ActionResponse> UpdateBookAsync(int id, UpdateBookModel book)
        {
            try
            {
                var bookToUpdate = await _context.Books.FindAsync(id);
                if (bookToUpdate == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy sách"
                    };
                }

                bookToUpdate.Name = book.Name ?? bookToUpdate.Name;
                bookToUpdate.ShortDescription = book.ShortDescription ?? bookToUpdate.ShortDescription;
                bookToUpdate.Description = book.Description ?? bookToUpdate.Description;
                bookToUpdate.Slug = book.Name != null ? new SlugHelper().GenerateSlug(book.Name) : bookToUpdate.Slug;
                bookToUpdate.ImageURL =
                    book.Image != null ? UploadImage.UploadSingleImage(book.Image) : bookToUpdate.ImageURL;
                bookToUpdate.IsEBook = book.IsEBook ?? false;
                bookToUpdate.Language = book.Language ?? bookToUpdate.Language;
                bookToUpdate.Pages = book.Pages ?? bookToUpdate.Pages;
                bookToUpdate.CategoryId = book.CategoryId ?? bookToUpdate.CategoryId;
                bookToUpdate.AuthorId = book.AuthorId ?? bookToUpdate.AuthorId;
                bookToUpdate.PublisherId = book.PublisherId ?? bookToUpdate.PublisherId;
                bookToUpdate.IsPublish = book.IsPublish;
                _context.Books.Update(bookToUpdate);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật thông tin sách thành công",
                    Data = bookToUpdate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Cập nhật sách thất bại"
                };
            }
        }

        public async Task<ActionResponse> ShowBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy sách"
                    };
                }

                book.IsPublish = true;
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Hiển thị sách thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Hiển thị sách thất bại"
                };
            }
        }

        public async Task<ActionResponse> HideBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy sách"
                    };
                }

                book.IsPublish = false;
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Đã ẩn sách thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Ẩn sách thất bại"
                };
            }
        }

        public async Task<ActionResponse> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy sách"
                    };
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Đã xóa sách thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Xóa sách thất bại"
                };
            }
        }
    }
}