using LewisStore.Models;

namespace LewisStore.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order, bool reserveStock = true);
    Task<Order?> GetByIdAsync(Guid id);
}
