using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Services;

namespace Smart_Library.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBooksService _booksService;

    public HomeController(ILogger<HomeController> logger, IBooksService booksService)
    {
        _logger = logger;
        _booksService = booksService;
    }

    public async Task<IActionResult> Index(int? page, int? pageSize)
    {
        var response = await _booksService.GetListBookAsync(page, pageSize);
        var data = response.Data as dynamic;
        ViewBag.TotalBooks = data?.totalBooks;
        ViewBag.TotalPage = data?.totalPages;
        ViewBag.currentPageSize = data?.currentPageSize;
        ViewBag.CurrentPage = data?.currentPage;
        var booksList = data?.books as List<BookViewModel>;
        return View(booksList);
    }

}

