using System.Diagnostics;
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
        public IActionResult Index(int? page, int? pageSize)
        {
            return NotFound();
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

