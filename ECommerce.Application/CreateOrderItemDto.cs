namespace ECommerce.Application.Orders.Commands;

public record CreateOrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);