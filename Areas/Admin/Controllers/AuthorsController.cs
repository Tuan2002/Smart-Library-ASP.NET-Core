using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Config;
using Smart_Library.Models;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = AppRoles.Admin)]
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorsManagerService _authorsManagerService;

        public AuthorsController(ILogger<AuthorsController> logger, IAuthorsManagerService authorsManagerService)
        {
            _logger = logger;
            _authorsManagerService = authorsManagerService;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var response = await _authorsManagerService.GetAuthorsAsync(page, pageSize);
            if (!response.IsSuccess)
            {
                return StatusCode(500);
            }
            var data = response.Data as dynamic;
            ViewBag.totalAuthors = data?.totalAuthors;
            ViewBag.TotalPages = data?.totalPages;
            ViewBag.CurrentPage = data?.currentPage;
            ViewBag.CurrentPageSize = data?.currentPageSize;
            var authors = data?.authors as List<AuthorViewModel>;
            return View(authors);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Author/Add")]
        public async Task<IActionResult> AddAuthor(CreateAuthorModel author)
        {
            var response = await _authorsManagerService.CreateAuthorAsync(author);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Authors");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Authors");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Author/Update")]
        public async Task<IActionResult> UpdateAuthor(UpdateAuthorModel author)
        {
            var response = await _authorsManagerService.UpdateAuthorAsync(author);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Authors");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Authors");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Author/Delete")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var response = await _authorsManagerService.DeleteAuthorAsync(authorId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Authors");
            }
            TempData["SystemMessage"] = response.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index", "Authors");
        }
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string? query)
        {
            var response = await _authorsManagerService.SearchAuthorsAsync(query);
            return Ok(response);
        }
    }
}