using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Models;
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
        private readonly ICategoriesManagerService _categoriesManagerService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService, IBooksManagerService booksManagerService, ICategoriesManagerService categoriesManagerService)
        {
            _logger = logger;
            _booksService = booksService;
            _booksManagerService = booksManagerService;
            _categoriesManagerService = categoriesManagerService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> Categories(int? page, int? pageSize)
        {
            var response = await _categoriesManagerService.GetCategoriesAsync(page, pageSize);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            var data = response.Data as dynamic;
            ViewBag.TotalCategories = data?.totalCategories;
            ViewBag.TotalPage = data?.totalPages;
            ViewBag.currentPageSize = data?.currentPageSize;
            ViewBag.CurrentPage = data?.currentPage;
            var categories = data?.categories as List<CategoryViewModel>;
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
            var result = await _categoriesManagerService.CreateCategoryAsync(category);
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
        public async Task<IActionResult> ShowCategory(int id)
        {
            var result = await _categoriesManagerService.ShowCategoryAsync(id);
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
        public async Task<IActionResult> HideCategory(int id)
        {
            var result = await _categoriesManagerService.HideCategoryAsync(id);
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
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
            var result = await _categoriesManagerService.UpdateCategoryAsync(id, name);
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoriesManagerService.DeleteCategoryAsync(id);
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