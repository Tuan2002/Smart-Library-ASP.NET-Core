using Microsoft.AspNetCore.Mvc;
using Smart_Library.Models;
using Smart_Library.Data;
using Smart_Library.Services;

namespace Smart_Library.Areas.Admin.Components
{
    [ViewComponent(Name = "Cart")]
    public class Cart : ViewComponent
    {
        private readonly ApplicationDBContext _context;
        private readonly IOrderServices _orderServices;
        public Cart(ApplicationDBContext context, IOrderServices orderServices)
        {
            _context = context;
            _orderServices = orderServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = _orderServices.GetCart();
            var cartItems = response.Data as List<CartItem>;
            return await Task.FromResult((IViewComponentResult)View(cartItems));
        }
    }
}