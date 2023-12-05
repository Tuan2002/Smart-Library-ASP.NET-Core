using System.Security.Claims;
using Slugify;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Data;
using Smart_Library.Entities;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IAuthorsManagerService
    {
        Task<ActionResponse> CreateAuthorAsync(CreateAuthorModel author);
        Task<ActionResponse> GetAuthorsAsync(int? page, int? pageSize);
        Task<ActionResponse> UpdateAuthorAsync(UpdateAuthorModel author);
        Task<ActionResponse> DeleteAuthorAsync(int authorId);
        Task<ActionResponse> SearchAuthorsAsync(string? searchQuery);
    }
    public class AuthorsManagerService : IAuthorsManagerService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthorsManagerService> _logger;
        public AuthorsManagerService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<AuthorsManagerService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ActionResponse> CreateAuthorAsync(CreateAuthorModel author)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Người dùng không hợp lệ"
                    };
                }
                var slugHelper = new SlugHelper();
                var authorImage = UploadImage.UploadSingleImage(author.Image) ?? "/img/default-user.webp";
                var newAuthor = new Author
                {
                    Name = author.Name,
                    Email = author.Email,
                    Slug = slugHelper.GenerateSlug(author.Name),
                    Address = author.GetAddress(),
                    ImageURL = authorImage,
                    Title = author.Title,
                    AddedAt = DateTime.UtcNow,
                    AddedById = userId
                };
                await _context.Author.AddAsync(newAuthor);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Thêm tác giả thành công"
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"AuthorsManagerService.CreateAuthorAsync at: {DateTime.UtcNow}");
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể thêm tác giả, vui lòng thử lại sau"
                };
            }
        }
        public async Task<ActionResponse> GetAuthorsAsync(int? page, int? pageSize)
        {
            try
            {
                var query = _context.Author.AsQueryable();
                var totalAuthors = await query.CountAsync();
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var totalPages = (int)Math.Ceiling((double)totalAuthors / currentPageSize);
                var authors = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(author => new AuthorViewModel()
                {
                    AuthorId = author.AuthorId,
                    Name = author.Name,
                    Address = author.Address,
                    Email = author.Email,
                    ImageURL = author.ImageURL,
                    Slug = author.Slug,
                    Title = author.Title,
                    AddedAt = author.AddedAt,
                    AddedByName = author.AddedBy.FirstName + " " + author.AddedBy.LastName
                }).ToListAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Lấy danh sách tác giả thành công",
                    Data = new
                    {
                        totalAuthors,
                        totalPages,
                        currentPage,
                        currentPageSize,
                        authors
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"AuthorsManagerService.GetAuthorsAsync at: {DateTime.UtcNow}");
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể lấy danh sách tác giả, vui lòng thử lại sau"
                };
            }
        }
        public async Task<ActionResponse> UpdateAuthorAsync(UpdateAuthorModel author)
        {
            try
            {
                var slugHelper = new SlugHelper();
                var authorToUpdate = await _context.Author.FindAsync(author.AuthorId);
                if (authorToUpdate == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy tác giả"
                    };
                }
                authorToUpdate.Name = author.Name ?? authorToUpdate.Name;
                authorToUpdate.Email = author.Email ?? authorToUpdate.Email;
                authorToUpdate.Slug = slugHelper.GenerateSlug(author.Name) ?? authorToUpdate.Slug;
                authorToUpdate.Address = author.Address ?? authorToUpdate.Address;
                authorToUpdate.ImageURL = UploadImage.UploadSingleImage(author.Image) ?? authorToUpdate.ImageURL;
                authorToUpdate.Title = author.Title ?? authorToUpdate.Title;
                _context.Author.Update(authorToUpdate);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật tác giả thành công"
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"AuthorsManagerService.UpdateAuthorAsync at: {DateTime.UtcNow}");
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể cập nhật tác giả, vui lòng thử lại sau"
                };
            }
        }
        public async Task<ActionResponse> DeleteAuthorAsync(int authorId)
        {
            try
            {
                var authorToDelete = await _context.Author.FindAsync(authorId);
                if (authorToDelete == null)
                {
                    return new ActionResponse
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy tác giả"
                    };
                }
                _context.Author.Remove(authorToDelete);
                await _context.SaveChangesAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Xóa tác giả thành công"
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"AuthorsManagerService.DeleteAuthorAsync at: {DateTime.UtcNow}");
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể xóa tác giả, vui lòng thử lại sau"
                };
            }
        }
        public async Task<ActionResponse> SearchAuthorsAsync(string? searchQuery)
        {
            try
            {
                var query = _context.Author.AsQueryable();
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(author => author.Name.Contains(searchQuery)).OrderByDescending(author => author.AddedAt);
                }
                query = query.Take(10);
                var authors = await query.Select(author => new AuthorViewModel()
                {
                    AuthorId = author.AuthorId,
                    Name = author.Name,
                    Address = author.Address,
                    Email = author.Email,
                    ImageURL = author.ImageURL,
                    Slug = author.Slug,
                    Title = author.Title,
                    AddedAt = author.AddedAt,
                }).ToListAsync();
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Tìm kiếm tác giả thành công",
                    Data = new
                    {
                        authors
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, $"AuthorsManagerService.SearchAuthorsAsync at: {DateTime.UtcNow}");
                return new ActionResponse
                {
                    IsSuccess = false,
                    Message = "Không thể tìm kiếm tác giả, vui lòng thử lại sau"
                };
            }
        }

    }
}