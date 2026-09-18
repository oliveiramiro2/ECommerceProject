using ECommerce.Domain.Common;
using ECommerce.Domain.ValueObjects;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Orders;
public class OrderItem : Entity<Guid>
{
  public Guid OrderId { get; private set; }
  public Guid ProductId { get; private set; }
  public string ProductName { get; private set; } = string.Empty;
  public Money UnitPrice { get; private set; } = null!;
  public int Quantity { get; private set; }
  public Money TotalPrice => new(UnitPrice.Amount * Quantity, UnitPrice.Currency);

  protected OrderItem() { }

  internal OrderItem(Guid orderId, Guid productId, string productName, Money unitPrice, int quantity)
  {
    Id = Guid.NewGuid();
    OrderId = orderId;
    ProductId = productId;
    ProductName = productName;
    UnitPrice = unitPrice;

    if (quantity <= 0) throw new DomainException("A quantidade do item deve ser maior que zero.");
    Quantity = quantity;
  }

  internal void AddQuantity(int quantity)
  {
    if (quantity <= 0) throw new DomainException("Quantidade inválida.");
    Quantity += quantity;
  }
}