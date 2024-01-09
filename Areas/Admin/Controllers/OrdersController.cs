using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smart_Library.Areas.Admin.Services;
using Smart_Library.Config;
using Smart_Library.Models;

namespace Smart_Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = AppRoles.Admin)]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersManagerService _ordersManagerService;
        public OrdersController(ILogger<OrdersController> logger, IOrdersManagerService ordersManagerService)
        {
            _logger = logger;
            _ordersManagerService = ordersManagerService;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var response = await _ordersManagerService.GetOrdersAsync(page, pageSize);
            if (!response.IsSuccess)
            {
                return StatusCode(500);
            }
            var data = response.Data as dynamic;
            ViewBag.TotalOrders = data?.totalOrders ?? 0;
            ViewBag.TotalPage = data?.totalPages ?? 1;
            ViewBag.currentPageSize = data?.currentPageSize ?? 10;
            ViewBag.CurrentPage = data?.currentPage ?? 1;
            var orders = data?.orders as List<OrderViewModel>;
            return View(orders);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> Details(int orderId)
        {
            var response = await _ordersManagerService.GetOrderDetailsAsync(orderId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            var orderDetails = response.Data as List<OrderDetailViewModel>;
            return View(orderDetails);
        }
        [HttpGet]
        [Route("ConfirmAll/{orderId}")]
        public async Task<IActionResult> ConfirmAll(int orderId)
        {
            var response = await _ordersManagerService.ConfirmAllOrderAsync(orderId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Xác nhận đơn mượn thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Xác nhận đơn mượn thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        [Route("Confirm/{orderDetailsId}")]
        public async Task<IActionResult> Confirm(int orderDetailsId)
        {
            var response = await _ordersManagerService.ConfirmOrderAsync(orderDetailsId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Xác nhận đơn mượn thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Xác nhận đơn mượn thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        [Route("Reject/{orderDetailsId}")]
        public async Task<IActionResult> Reject(int orderDetailsId)
        {
            var response = await _ordersManagerService.RejectOrderAsync(orderDetailsId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Từ chối đơn mượn thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Từ chối đơn mượn thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        [Route("ReturnAll/{orderId}")]
        public async Task<IActionResult> ReturnAll(int orderId)
        {
            var response = await _ordersManagerService.ReturnAllOrderAsync(orderId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Trả sách thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Trả sách thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        [Route("Return/{orderDetailsId}")]
        public async Task<IActionResult> Return(int orderDetailsId)
        {
            var response = await _ordersManagerService.ReturnOrderAsync(orderDetailsId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Trả sách thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Trả sách thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await _ordersManagerService.DeleteOrderAsync(orderId);
            if (!response.IsSuccess)
            {
                TempData["SystemMessage"] = response?.Message ?? "Xóa đơn mượn thất bại";
                TempData["Type"] = "error";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            TempData["SystemMessage"] = response?.Message ?? "Xóa đơn mượn thành công";
            TempData["Type"] = "success";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}