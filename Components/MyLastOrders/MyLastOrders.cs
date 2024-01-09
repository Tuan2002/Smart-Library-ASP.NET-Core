using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Data;
using Smart_Library.Services;
using System.Security.Claims;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "MyLastOrders")]
    public class MyLastOrders : ViewComponent
    {
        private readonly IOrderServices _orderServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MyLastOrders(IOrderServices orderServices, IHttpContextAccessor httpContextAccessor)
        {
            _orderServices = orderServices;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var response = await _orderServices.GetLastOrderDetailsAsync(userId);
            if (!response.IsSuccess)
            {
                return await Task.FromResult((IViewComponentResult)View(new List<OrderDetailViewModel>()));
            }
            var orderDetails = response.Data as List<OrderDetailViewModel>;
            return await Task.FromResult((IViewComponentResult)View(orderDetails));
        }
    }
}