using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Config;
using Smart_Library.Data;
using Smart_Library.Entities;
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
        Task<ActionResponse> CreateOrderAsync();
        Task<ActionResponse> CancelOrderAsync(int orderId);
        Task<ActionResponse> GetLastOrderDetailsAsync(string userId);
        Task<ActionResponse> GetMyOrdersAsync(int? page, int? pageSize);
        Task<ActionResponse> GetMyOrderDetailsAsync(int orderId);
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
        public async Task<ActionResponse> CreateOrderAsync()
        {
            var currentCart = cart;
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse { IsSuccess = false, Message = "Người dùng không tồn tại" };
                }
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    TotalBook = currentCart.Sum(x => x.Quantity),
                    StatusId = 1
                };
                _context.Database.BeginTransaction();
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                foreach (var cartItem in currentCart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity,
                        OrderDate = DateTime.Now,
                        ReturnDate = DateTime.Now.AddDays(cartItem.NumOfDay),
                        NumOfDay = cartItem.NumOfDay,
                        StatusId = 1
                    };
                    await _context.OrderDetails.AddAsync(orderDetail);
                    await _context.SaveChangesAsync();
                }
                _context.Database.CommitTransaction();
                _httpContextAccessor?.HttpContext?.Session.Remove(SessionKey.CART_KEY);
                return new ActionResponse { IsSuccess = true, Message = "Tạo đơn mượn sách thành công" };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError(ex, "CreateOrderAsync");
                return new ActionResponse { IsSuccess = false, Message = "Tạo đơn mượn sách thất bại" };
            }
        }
        public async Task<ActionResponse> GetLastOrderDetailsAsync(string userId)
        {
            try
            {
                var order = await _context.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.CreateDate).Take(2).ToListAsync();
                if (order == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Không tìm thấy đơn mượn sách" };
                }
                var orderDetailsList = new List<OrderDetailViewModel>();
                foreach (var item in order)
                {
                    var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == item.OrderId).ToListAsync();
                    orderDetailsList.AddRange(_mapper.Map<List<OrderDetailViewModel>>(orderDetails));
                }
                return new ActionResponse { IsSuccess = true, Message = "Lấy đơn mượn sách thành công", Data = orderDetailsList };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetLastOrderDetailsAsync");
                return new ActionResponse { IsSuccess = false, Message = "Lấy đơn mượn sách thất bại" };
            }

        }
        public async Task<ActionResponse> GetMyOrdersAsync(int? page, int? pageSize)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse { IsSuccess = false, Message = "Người dùng không tồn tại" };
                }
                var query = _context.Orders.AsQueryable();
                query = query.Where(x => x.UserId == userId);
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var totalOrders = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalOrders / currentPageSize);
                var orders = await query.OrderByDescending(x => x.CreateDate).Skip((currentPage - 1) * currentPageSize).Take(currentPageSize).ToListAsync();
                var orderList = _mapper.Map<List<OrderViewModel>>(orders);
                return new ActionResponse
                {
                    IsSuccess = true,
                    Message = "Lấy đơn mượn sách thành công",
                    Data = new
                    {
                        orders = orderList,
                        currentPage,
                        currentPageSize,
                        totalPages,
                        totalOrders
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMyOrdersAsync");
                return new ActionResponse { IsSuccess = false, Message = "Lấy đơn mượn sách thất bại" };
            }
        }
        public async Task<ActionResponse> CancelOrderAsync(int orderId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse { IsSuccess = false, Message = "Người dùng không tồn tại" };
                }
                var order = await _context.Orders.FindAsync(orderId);
                if (userId != order?.UserId)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Bạn không có quyền hủy đơn mượn sách này" };
                }
                if (order == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Đơn mượn sách không tồn tại" };
                }
                order.StatusId = 6;
                var orderList = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
                foreach (var item in orderList)
                {
                    item.StatusId = 6;
                }
                _context.Database.BeginTransaction();
                _context.Orders.Update(order);
                _context.OrderDetails.UpdateRange(orderList);
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                return new ActionResponse { IsSuccess = true, Message = "Hủy đơn mượn sách thành công" };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError(ex, "CancelOrderAsync");
                return new ActionResponse { IsSuccess = false, Message = "Hủy đơn mượn sách thất bại" };
            }
        }
        public async Task<ActionResponse> GetMyOrderDetailsAsync(int orderId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                    return new ActionResponse { IsSuccess = false, Message = "Người dùng không tồn tại" };
                }
                var order = await _context.Orders.FindAsync(orderId);
                if (userId != order?.UserId)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Bạn không có quyền xem đơn mượn sách này" };
                }
                if (order == null)
                {
                    return new ActionResponse { IsSuccess = false, Message = "Đơn mượn sách không tồn tại" };
                }
                var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
                var orderDetailsList = _mapper.Map<List<OrderDetailViewModel>>(orderDetails);
                return new ActionResponse { IsSuccess = true, Message = "Lấy đơn mượn sách thành công", Data = orderDetailsList };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMyOrderDetailsAsync");
                return new ActionResponse { IsSuccess = false, Message = "Lỗi hệ thống, vui lòng thử lại sau" };
            }
        }
    }
}