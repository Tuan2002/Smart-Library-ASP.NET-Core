using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Services;

namespace Smart_Library.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBooksService _booksService;
        private readonly IOrderServices _orderServices;

        public HomeController(ILogger<HomeController> logger, IBooksService booksService, IOrderServices orderServices)
        {
            _logger = logger;
            _booksService = booksService;
            _orderServices = orderServices;
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
        [HttpGet]
        [Route("My-profile")]
        public IActionResult MyProfile()
        {
            return View();
        }
        [HttpGet]
        [Route("My-orders")]
        public async Task<IActionResult> MyOrders(int? page, int? pageSize)
        {
            var response = await _orderServices.GetMyOrdersAsync(page, pageSize);
            var data = response.Data as dynamic;
            ViewBag.TotalOrders = data?.totalOrders;
            ViewBag.TotalPage = data?.totalPages;
            ViewBag.currentPageSize = data?.currentPageSize;
            ViewBag.CurrentPage = data?.currentPage;
            var ordersList = data?.orders as List<OrderViewModel>;
            return View(ordersList);
        }
        [HttpGet]
        [Route("My-orders/{orderId:int}")]
        public async Task<IActionResult> MyOrderDetails(int orderId)
        {
            var response = await _orderServices.GetMyOrderDetailsAsync(orderId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Lấy thông tin lịch sử thất bại";
                TempData["Type"] = "error";
                return RedirectToAction("MyOrders", "Home");
            }
            return View(response.Data as List<OrderDetailViewModel>);
        }
    }

}