using Application.Dashboard.Dtos;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, bool includeDetails = false)
        {
            var query = _context.Orders.AsQueryable();

            if (includeDetails)
            {
                query = query
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.Payments);
            }

            return await query.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(bool includeDetails = false)
        {
            var query = _context.Orders.AsQueryable();

            if (includeDetails)
            {
                query = query
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Include(o => o.Payments);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(int userId, bool includeDetails = false)
        {
            var query = _context.Orders.Where(o => o.UserId == userId);

            if (includeDetails)
            {
                query = query
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.Payments);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(string status, bool includeDetails = false)
        {
            if (!Enum.TryParse<OrderStatus>(status, out var orderStatus))
                return new List<Order>();

            var query = _context.Orders.Where(o => o.Status == orderStatus);

            if (includeDetails)
            {
                query = query
                    .Include(o => o.User)
                    .Include(o => o.Address)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.Payments);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Payment>> GetAllPaymentsAsync(int? orderId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Payments.AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(p => p.OrderId == orderId.Value);
            }

            return await query
                .OrderByDescending(p => p.PaymentDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }


        // تقارير
        public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == OrderStatus.Delivered)
                .SumAsync(o => o.TotalAmount);
        }
        public async Task<List<Order>> GetOrdersDailyReport(DateTime startOfDay, DateTime endOfDay, CancellationToken cancellationToken)
        {
            return await _context.Orders.Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay)
                           .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<string, decimal>> GetSalesByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay && o.Status == OrderStatus.Delivered)
                .ToListAsync();

            return new Dictionary<string, decimal>
            {
                { "TotalSales", orders.Sum(o => o.TotalAmount) },
                { "OrderCount", orders.Count }
            };
        }

        public async Task<Dictionary<string, decimal>> GetMonthlySalesAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate && o.Status == OrderStatus.Delivered)
                .ToListAsync();

            return new Dictionary<string, decimal>
            {
                { "TotalSales", orders.Sum(o => o.TotalAmount) },
                { "OrderCount", orders.Count }
            };
        }

        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<int> GetPendingOrdersCountAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        }

        public async Task<List<Order>> GetOrdersDeliveredAsync(CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .ToListAsync(cancellationToken);
        }

    }
}