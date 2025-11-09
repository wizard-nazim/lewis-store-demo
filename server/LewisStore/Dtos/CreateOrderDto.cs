namespace LewisStore.Dtos;

public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
public record CreateOrderDto(decimal DeliveryFee, decimal Tax, string PaymentType, List<OrderItemDto> Items);
