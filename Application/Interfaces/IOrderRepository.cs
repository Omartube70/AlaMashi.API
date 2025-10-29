using Application.Dashboard.Dtos;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int orderId, bool includeDetails = false);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync(bool includeDetails = false);
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(int userId, bool includeDetails = false);
        Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(string status, bool includeDetails = false);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);

        // تقارير
        Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetOrdersDailyReport(DateTime startOfDay, DateTime endOfDay, CancellationToken cancellationToken);// تقارير يومية
        Task<Dictionary<string, decimal>> GetSalesByDateAsync(DateTime date);
        Task<Dictionary<string, decimal>> GetMonthlySalesAsync(int year, int month);
        Task<int> GetTotalOrdersCountAsync();
        Task<int> GetPendingOrdersCountAsync();
        Task<List<Order>> GetOrdersDeliveredAsync(CancellationToken cancellationToken);
    }
}

