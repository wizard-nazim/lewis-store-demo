using Microsoft.EntityFrameworkCore;
using LewisStore.Data;
using LewisStore.Models;

namespace LewisStore.Services;

public class OrderService : IOrderService
{
    private readonly LewisDbContext _db;
    private readonly ICreditService _creditService;

    public OrderService(LewisDbContext db, ICreditService creditService)
    {
        _db = db;
        _creditService = creditService;
    }

    public async Task<Order> CreateOrderAsync(Order order, bool reserveStock = true)
    {
        order.Subtotal = order.Items?.Sum(i => i.LineTotal) ?? 0m;
        order.Total = order.Subtotal + order.DeliveryFee + order.Tax;

        var customer = await _db.Users.FindAsync(order.CustomerId);
        if (customer == null) throw new InvalidOperationException("Customer not found");

        if (order.PaymentType == "Cash")
        {
            if (customer.Balance < order.Total) throw new InvalidOperationException("Insufficient funds");
            customer.Balance -= order.Total;
            _db.Payments.Add(new Payment { OrderId = order.Id, Amount = order.Total, Method = "Wallet", Reference = $"AutoPayment-{order.Id}" });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        if (reserveStock)
        {
            foreach (var item in order.Items ?? Array.Empty<OrderItem>())
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQty -= item.Quantity;
                    _db.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductId = product.Id,
                        ChangeQty = -item.Quantity,
                        Type = "Sale",
                        Note = $"Order {order.Id}"
                    });
                }
            }
            await _db.SaveChangesAsync();
        }

        if (order.PaymentType == "Credit")
        {
            var principal = order.Total;
            await _creditService.CreateAgreementAsync(order.Id, principal, 12, 0.20m);
        }

        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid id) =>
        await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
}

//Please review this code for correctness and completeness