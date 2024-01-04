using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Smart_Library.Areas.Admin.Models;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Config;
using Smart_Library.Models;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = AppRoles.Admin)]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksManagerService _booksManagerService;
        private readonly ICategoriesManagerService _categoriesManagerService;
        private readonly IAuthorsManagerService _authorsManagerService;
        private readonly IPublisherManagerService _publisherManagerService;

        public BooksController(ILogger<BooksController> logger, IBooksManagerService booksManagerService, ICategoriesManagerService categoriesManagerService, IAuthorsManagerService authorsManagerService, IPublisherManagerService publisherManagerService)
        {
            _logger = logger;
            _booksManagerService = booksManagerService;
            _categoriesManagerService = categoriesManagerService;
            _authorsManagerService = authorsManagerService;
            _publisherManagerService = publisherManagerService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var response = await _booksManagerService.GetBooksAsync(page, pageSize);
            if (!response.IsSuccess)
            {
                return StatusCode(500);
            }
            var data = response.Data as dynamic;
            ViewBag.TotalBooks = data?.totalBooks ?? 0;
            ViewBag.TotalPage = data?.totalPages ?? 1;
            ViewBag.currentPageSize = data?.currentPageSize ?? 10;
            ViewBag.CurrentPage = data?.currentPage ?? 1;
            var books = data?.books as List<BookViewModel>;
            return View(books);
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
            ViewBag.TotalCategories = data?.totalCategories ?? 0;
            ViewBag.TotalPage = data?.totalPages ?? 1;
            ViewBag.currentPageSize = data?.currentPageSize ?? 10;
            ViewBag.CurrentPage = data?.currentPage ?? 1;
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [HttpGet]
        [Route("Create/Book")]
        public async Task<IActionResult> CreateBook()
        {
            var categories = await _categoriesManagerService.GetListCategoryAsync();
            var authors = await _authorsManagerService.GetListAuthorAsync();
            var publishers = await _publisherManagerService.GetListPublisherAsync();
            if (!categories.IsSuccess || !authors.IsSuccess || !publishers.IsSuccess)
            {
                return StatusCode(500);
            }
            ViewBag.Categories = new SelectList(categories.Data as List<CategoryViewModel>, "CategoryId", "Name");
            ViewBag.Authors = new SelectList(authors.Data as List<AuthorViewModel>, "AuthorId", "Name");
            ViewBag.Publishers = new SelectList(publishers.Data as List<PublisherViewModel>, "PublisherId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create/Book")]
        public async Task<IActionResult> CreateBook(CreateBookModel book)
        {
            if (!ModelState.IsValid)
            {
                TempData["SystemMessage"] = "Thông tin sách không hợp lệ";
                TempData["Type"] = "error";
                return RedirectToAction("CreateBook");
            }
            var result = await _booksManagerService.CreateBookAsync(book);
            if (!result.IsSuccess)
            {
                TempData["SystemMessage"] = result.Message;
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = result.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("Show/Book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowBook(int id)
        {
            var result = await _booksManagerService.ShowBookAsync(id);
            if (result.IsSuccess)
            {
                TempData["SystemMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("Hide/Book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HideBook(int id)
        {
            var result = await _booksManagerService.HideBookAsync(id);
            if (result.IsSuccess)
            {
                TempData["SystemMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Update/Book/{id:int}")]
        public async Task<IActionResult> UpdateBook(int id)
        {
            var response = await _booksManagerService.GetBookByIdAsync(id);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            var categories = await _categoriesManagerService.GetListCategoryAsync();
            var authors = await _authorsManagerService.GetListAuthorAsync();
            var publishers = await _publisherManagerService.GetListPublisherAsync();
            if (!categories.IsSuccess || !authors.IsSuccess || !publishers.IsSuccess)
            {
                return NotFound();
            }
            var data = response.Data as BookViewModel;
            ViewBag.Categories = new SelectList(categories.Data as List<CategoryViewModel>, "CategoryId", "Name", data?.CategoryId);
            ViewBag.Authors = new SelectList(authors.Data as List<AuthorViewModel>, "AuthorId", "Name", data?.AuthorId);
            ViewBag.Publishers = new SelectList(publishers.Data as List<PublisherViewModel>, "PublisherId", "Name", data?.PublisherId);
            return View((data, new UpdateBookModel()));
        }
        [HttpPost]
        [Route("Update/Book/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBook(int id, [Bind(Prefix = "Item2")] UpdateBookModel book)
        {
            if (!ModelState.IsValid)
            {
                TempData["SystemMessage"] = "Thông tin sách không hợp lệ";
                TempData["Type"] = "error";
                return RedirectToAction("UpdateBook", new { id });
            }
            var result = await _booksManagerService.UpdateBookAsync(id, book);
            if (!result.IsSuccess)
            {
                TempData["SystemMessage"] = result.Message;
                TempData["Type"] = "error";
                return RedirectToAction("UpdateBook", new { id });
            }
            TempData["SystemMessage"] = result.Message;
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("Delete/Book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _booksManagerService.DeleteBookAsync(id);
            if (result.IsSuccess)
            {
                TempData["SystemMessage"] = result.Message;
                TempData["Type"] = "success";
                return RedirectToAction("Index");
            }
            TempData["SystemMessage"] = result.Message;
            TempData["Type"] = "error";
            return RedirectToAction("Index");
        }
    }
}