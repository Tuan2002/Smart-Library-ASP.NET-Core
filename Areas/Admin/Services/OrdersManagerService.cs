
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Data;
using Smart_Library.Models;
using Smart_Library.Utils;

namespace Smart_Library.Areas.Admin.Services
{
    public interface IOrdersManagerService
    {
        Task<ActionResponse> GetOrdersAsync(int? page, int? pageSize);
        Task<ActionResponse> GetOrderDetailsAsync(int orderId);
        Task<ActionResponse> ConfirmAllOrderAsync(int orderId);
        Task<ActionResponse> ConfirmOrderAsync(int orderDetailsId);
        Task<ActionResponse> RejectOrderAsync(int orderDetailsId);
        Task<ActionResponse> ReturnAllOrderAsync(int orderId);
        Task<ActionResponse> ReturnOrderAsync(int orderDetailsId);
        Task<ActionResponse> DeleteOrderAsync(int orderId);

    }
    public class OrdersManagerService : IOrdersManagerService
    {
        private readonly ILogger<OrdersManagerService> _logger;
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public OrdersManagerService(ILogger<OrdersManagerService> logger, ApplicationDBContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResponse> GetOrdersAsync(int? page, int? pageSize)
        {
            try
            {
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                var query = _context.Orders.AsQueryable();
                var totalOrders = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalOrders / currentPageSize);
                var orderList = await query
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToListAsync();
                var orders = _mapper.Map<List<OrderViewModel>>(orderList);
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Lấy danh sách đơn mượn thành công",
                    Data = new
                    {
                        totalOrders,
                        totalPages,
                        currentPage,
                        currentPageSize,
                        orders
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetOrdersAsync Error.");
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Lấy danh sách đơn mượn thất bại"
                };
            }
        }
        public async Task<ActionResponse> GetOrderDetailsAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                var orderList = order.OrderDetails.ToList();
                var orderDetails = _mapper.Map<List<OrderDetailViewModel>>(orderList);
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Lấy danh sách chi tiết đơn mượn thành công",
                    Data = orderDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetOrderDetailsAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Lấy danh sách chi tiết đơn mượn thất bại"
                };
            }
        }
        public async Task<ActionResponse> ConfirmAllOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                order.StatusId = 3;
                var orderList = await _context.OrderDetails.Where(x => x.OrderId == orderId && x.StatusId < 3).ToListAsync();
                foreach (var item in orderList)
                {
                    item.StatusId = 3;
                    item.Book.ReaderCount++;
                }
                _context.Database.BeginTransaction();
                _context.Orders.Update(order);
                _context.OrderDetails.UpdateRange(orderList);
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Xác nhận đơn mượn thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("ConfirmAllOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Xác nhận đơn mượn thất bại"
                };
            }

        }
        public async Task<ActionResponse> ConfirmOrderAsync(int orderDetailsId)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailsId);
                if (orderDetail == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                orderDetail.StatusId = 3;
                orderDetail.Book.ReaderCount++;
                _context.Database.BeginTransaction();
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
                var order = await _context.Orders.FindAsync(orderDetail.OrderId);
                if (order == null)
                {
                    _context.Database.RollbackTransaction();
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                if (order.OrderDetails.Any(x => x.StatusId == 3))
                {
                    order.StatusId = 3;
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                }
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Xác nhận đơn mượn thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("ConfirmOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Xác nhận đơn mượn thất bại"
                };
            }

        }
        public async Task<ActionResponse> RejectOrderAsync(int orderDetailsId)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailsId);
                if (orderDetail == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                orderDetail.StatusId = 6;
                _context.Database.BeginTransaction();
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
                var order = await _context.Orders.FindAsync(orderDetail.OrderId);
                if (order == null)
                {
                    _context.Database.RollbackTransaction();
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                if (order.OrderDetails.All(x => x.StatusId == 6))
                {
                    order.StatusId = 6;
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                }
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Từ chối đơn mượn thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("ConfirmOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Từ chối đơn mượn thất bại"
                };
            }
        }
        public async Task<ActionResponse> ReturnAllOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                order.StatusId = 4;
                var orderList = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
                foreach (var item in orderList)
                {
                    item.StatusId = 4;
                }
                _context.Database.BeginTransaction();
                _context.Orders.Update(order);
                _context.OrderDetails.UpdateRange(orderList);
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Xác nhận trả sách thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("ReturnAllOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Trả đơn mượn thất bại"
                };
            }
        }
        public async Task<ActionResponse> ReturnOrderAsync(int orderDetailsId)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailsId);
                if (orderDetail == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                orderDetail.StatusId = 4;
                _context.Database.BeginTransaction();
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
                var order = await _context.Orders.FindAsync(orderDetail.OrderId);
                if (order == null)
                {
                    _context.Database.RollbackTransaction();
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                if (!order.OrderDetails.Any(x => x.StatusId < 4))
                {
                    order.StatusId = 4;
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                }
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Xác nhận trả sách thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("ReturnOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Trả đơn mượn thất bại"
                };
            }
        }
        public async Task<ActionResponse> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return new ActionResponse()
                    {
                        IsSuccess = false,
                        Message = "Đơn mượn không tồn tại"
                    };
                }
                _context.Database.BeginTransaction();
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                return new ActionResponse()
                {
                    IsSuccess = true,
                    Message = "Xóa đơn mượn thành công"
                };
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError("DeleteOrderAsync Error.", ex.Message);
                return new ActionResponse()
                {
                    IsSuccess = false,
                    Message = "Xóa đơn mượn thất bại"
                };
            }
        }
    }
}