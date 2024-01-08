using AutoMapper;
using Smart_Library.Config;
using Smart_Library.Data;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Services
{
    public interface IOrderServices
    {
        ActionResponse GetCart();
        Task<ActionResponse> AddToCartAsync(int bookId);
        Task<ActionResponse> RemoveFromCartAsync(int bookId);
        Task<ActionResponse> UpdateQuantityAsync(int bookId, string type = "increase");
        Task<ActionResponse> UpdateDayAsync(int bookId, string type = "increase");
    }
    public class OrderServices : IOrderServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        public List<CartItem> cart => _httpContextAccessor?.HttpContext?.Session.Get<List<CartItem>>(SessionKey.CART_KEY) ?? new List<CartItem>();
        public OrderServices(IHttpContextAccessor httpContextAccessor, ILogger<BooksService> logger, ApplicationDBContext context, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public ActionResponse GetCart()
        {
            if (cart == null || cart.Count == 0)
            {
                return new ActionResponse { IsSuccess = false, Message = "Giỏ sách hiện tại đang trống" };
            }
            return new ActionResponse { IsSuccess = true, Message = "Lấy giỏ sách thành công", Data = cart };
        }
        public async Task<ActionResponse> AddToCartAsync(int bookId)
        {
            var currentCart = cart;
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại" };
                }
                var cartItem = currentCart.FirstOrDefault(item => item.BookId == bookId);
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        BookId = bookId,
                        Book = _mapper.Map<BookViewModel>(book),
                        Quantity = 1,
                        NumOfDay = 1
                    };
                    currentCart.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }
                _httpContextAccessor?.HttpContext?.Session.Set(SessionKey.CART_KEY, currentCart);
                return new ActionResponse { IsSuccess = true, Message = "Thêm vào giỏ sách thành công" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddToCartAsync");
                return new ActionResponse { IsSuccess = false, Message = "Thêm vào giỏ sách thất bại" };
            }
        }
        public async Task<ActionResponse> RemoveFromCartAsync(int bookId)
        {
            var currentCart = cart;
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại" };
                }
                var cartItem = currentCart.FirstOrDefault(x => x.BookId == bookId);
                if (cartItem == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại trong giỏ sách" };
                }
                currentCart.Remove(cartItem);
                _httpContextAccessor?.HttpContext?.Session.Set(SessionKey.CART_KEY, currentCart);
                return new ActionResponse { IsSuccess = true, Message = "Xóa khỏi giỏ sách thành công" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveFromCartAsync");
                return new ActionResponse { IsSuccess = false, Message = "Xóa khỏi giỏ sách thất bại" };
            }
        }
        public async Task<ActionResponse> UpdateQuantityAsync(int bookId, string type = "increase")
        {
            var currentCart = cart;
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại" };
                }
                var cartItem = currentCart.FirstOrDefault(x => x.BookId == bookId);
                if (cartItem == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại trong giỏ sách" };
                }
                switch (type)
                {
                    case "increase":
                        cartItem.Quantity++;
                        break;
                    case "decrease":
                        if (cartItem.Quantity == 1)
                            break;
                        cartItem.Quantity--;
                        break;
                    default:
                        return new ActionResponse { IsSuccess = false, Message = "Kiểu cập nhật không hợp lệ" };
                }
                _httpContextAccessor?.HttpContext?.Session.Set(SessionKey.CART_KEY, currentCart);
                return new ActionResponse { IsSuccess = true, Message = "Cập nhật giỏ sách thành công" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateQuantityAsync");
                return new ActionResponse { IsSuccess = false, Message = "Cập nhật giỏ sách thất bại" };
            }

        }
        public async Task<ActionResponse> UpdateDayAsync(int bookId, string type = "increase")
        {
            var currentCart = cart;
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại" };
                }
                var cartItem = currentCart.FirstOrDefault(x => x.BookId == bookId);
                if (cartItem == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Sách không tồn tại trong giỏ sách" };
                }
                switch (type)
                {
                    case "increase":
                        cartItem.NumOfDay++;
                        break;
                    case "decrease":
                        if (cartItem.NumOfDay == 1)
                            break;
                        cartItem.NumOfDay--;
                        break;
                    default:
                        return new ActionResponse { IsSuccess = false, Message = "Kiểu cập nhật không hợp lệ" };
                }
                _httpContextAccessor?.HttpContext?.Session.Set(SessionKey.CART_KEY, currentCart);
                return new ActionResponse { IsSuccess = true, Message = "Cập nhật giỏ sách thành công" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateQuantityAsync");
                return new ActionResponse { IsSuccess = false, Message = "Cập nhật giỏ sách thất bại" };
            }

        }
    }
}