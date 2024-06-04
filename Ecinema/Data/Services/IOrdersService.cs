using Ecinema.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecinema.Data.Services
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
        Task CancelOrderAsync(int orderId); // New method for canceling orders
        Task<Order> GetOrderByIdAsync(int orderId); // New method for getting order by ID
        Task EditOrderAsync(Order order); // New method for editing orders
    }
}
