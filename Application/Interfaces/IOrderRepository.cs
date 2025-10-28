using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<IReadOnlyList<Order>> GetUserOrdersAsync(int userId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
    }
}