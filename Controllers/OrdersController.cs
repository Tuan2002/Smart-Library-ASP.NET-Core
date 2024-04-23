using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smart_Library.Models;
using Smart_Library.Services;

namespace Smart_Library.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderServices _orderServices;

        public OrdersController(ILogger<OrdersController> logger, IOrderServices orderServices)
        {
            _logger = logger;
            _orderServices = orderServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Cart")]
        public IActionResult Cart()
        {
            var response = _orderServices.GetCart();
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Lấy giỏ sách thất bại";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }
            return View(response.Data as List<CartItem>);
        }
        [HttpGet]
        [Route("AddToCart/{bookId:int}")]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var response = await _orderServices.AddToCartAsync(bookId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Thêm sách vào giỏ thất bại";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }
            TempData["SystemMessage"] = response?.Message ?? "Thêm sách vào giỏ thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        [Route("RemoveFromCart/{bookId:int}")]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            var response = await _orderServices.RemoveFromCartAsync(bookId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Xóa sách khỏi giỏ thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Xóa sách khỏi giỏ thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPut]
        [Route("UpdateQuantity/{bookId:int}")]
        public async Task<IActionResult> UpdateQuantity(int bookId, string type = "increase")
        {
            var response = await _orderServices.UpdateQuantityAsync(bookId, type);
            return Ok(response);
        }
        [HttpPut]
        [Route("UpdateDay/{bookId:int}")]
        public async Task<IActionResult> UpdateDay(int bookId, string type = "increase")
        {
            var response = await _orderServices.UpdateDayAsync(bookId, type);
            return Ok(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            var response = await _orderServices.CreateOrderAsync();
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Tạo đơn mượn sách thất bại";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }
            TempData["SystemMessage"] = response?.Message ?? "Đăng ký mượn sách thành công";
            TempData["Type"] = "success";
            return RedirectToAction("MyOrders", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderServices.CancelOrderAsync(orderId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Hủy đơn mượn sách thất bại";
                TempData["Type"] = "error";
                return RedirectToAction("Index", "Home");
            }
            TempData["SystemMessage"] = response?.Message ?? "Hủy đơn mượn sách thành công";
            TempData["Type"] = "success";
            return RedirectToAction("MyOrders", "Home");
        }

    }
}