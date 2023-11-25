using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Services;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = "Quản trị viên")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;
        private readonly IBooksManagerService _booksManagerService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService, IBooksManagerService booksManagerService)
        {
            _logger = logger;
            _booksService = booksService;
            _booksManagerService = booksManagerService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> Categories()
        {
            var categories = await _booksService.GetCategoriesAsync();
            return View(categories);
        }
        [HttpPost]
        [Route("Create/Category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                TempData["CategoryMessage"] = "Tên danh mục không hợp lệ";
                TempData["Type"] = "error";
                return RedirectToAction("Categories");
            }
            var result = await _booksManagerService.CreateCategoryAsync(category);
            if (result.IsSuccess)
            {
                TempData["CategoryMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Categories");
            }
            TempData["CategoryMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Categories");
        }
        [HttpPost]
        [Route("Show/Category")]
        public async Task<IActionResult> ShowCategory(string id)
        {
            var result = await _booksManagerService.ShowCategoryAsync(id);
            if (result.IsSuccess)
            {
                TempData["CategoryMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Categories");
            }
            TempData["CategoryMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Categories");
        }
        [HttpPost]
        [Route("Hide/Category")]
        public async Task<IActionResult> HideCategory(string id)
        {
            var result = await _booksManagerService.HideCategoryAsync(id);
            if (result.IsSuccess)
            {
                TempData["CategoryMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Categories");
            }
            TempData["CategoryMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Categories");
        }
        [HttpPost]
        [Route("Update/Category")]
        public async Task<IActionResult> UpdateCategory(string id, string name)
        {
            var result = await _booksManagerService.UpdateCategoryAsync(id, name);
            if (result.IsSuccess)
            {
                TempData["CategoryMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Categories");
            }
            TempData["CategoryMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Categories");
        }
        [HttpPost]
        [Route("Delete/Category")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var result = await _booksManagerService.DeleteCategoryAsync(id);
            if (result.IsSuccess)
            {
                TempData["CategoryMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Categories");
            }
            TempData["CategoryMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Categories");
        }

    }
}