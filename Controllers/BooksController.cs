using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Services;

namespace Smart_Library.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService)
        {
            _logger = logger;
            _booksService = booksService;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize, string? categoryName, string? authorName)
        {
            var decodeCategoryName = HttpUtility.UrlDecode(categoryName);
            var decodeAuthorName = HttpUtility.UrlDecode(authorName);
            var response = await _booksService.GetListBookAsync(page, pageSize, decodeCategoryName, authorName);
            var data = response.Data as dynamic;
            ViewBag.CategoryName = decodeCategoryName ?? null;
            ViewBag.AuthorName = decodeAuthorName;
            ViewBag.TotalBooks = data?.totalBooks;
            ViewBag.TotalPage = data?.totalPages;
            ViewBag.currentPageSize = data?.currentPageSize;
            ViewBag.CurrentPage = data?.currentPage;
            var booksList = data?.books as List<BookViewModel>;
            if (booksList == null || booksList.Count == 0)
            {
                return NotFound();
            }
            return View(booksList);
        }
        [HttpGet]
        [Route("{id}/{slug}")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _booksService.GetBookDetailAsync(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return View(response.Data as BookViewModel);
        }
    }
}

