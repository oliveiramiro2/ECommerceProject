using System;
using System.Collections.Generic;
using System.Linq;

using ECommerce.Domain.Common;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Orders;

public class Order : ECommerce.Domain.Common.AggregateRoot<Guid>
{
  private readonly List<OrderItem> _items = [];

  public Guid CustomerId { get; private set; }
  public OrderStatus Status { get; private set; }
  public ECommerce.Domain.ValueObjects.Money TotalAmount => new(_items.Sum(i => i.TotalPrice.Amount));

  // Propriedade para Concorrência Otimista mapeada no EF Core
  public uint Version { get; private set; }

  // Construtor para o EF Core / ORM
  protected Order() { }

  public Order(Guid customerId)
  {
    Id = Guid.NewGuid();
    CustomerId = customerId;
    Status = OrderStatus.Pending;

    AddDomainEvent(new OrderCreatedDomainEvent(Id, CustomerId));
  }

  public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

  public void AddItem(Guid productId, string productName, ECommerce.Domain.ValueObjects.Money unitPrice, int quantity)
  {
    if (Status != OrderStatus.Pending)
      throw new DomainException("Não é possível alterar um pedido que não está pendente.");

    var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
    if (existingItem is not null)
    {
      existingItem.AddQuantity(quantity);
    }
    else
    {
      _items.Add(new OrderItem(Id, productId, productName, unitPrice, quantity));
    }
  }

  public void MarkAsPaid()
  {
    if (Status != OrderStatus.Pending)
      throw new DomainException("Apenas pedidos pendentes podem ser pagos.");

    if (!_items.Any())
      throw new DomainException("Não é possível pagar um pedido sem itens.");

    Status = OrderStatus.Paid;
    AddDomainEvent(new OrderPaidDomainEvent(Id));
  }

  public void Cancel()
  {
    if (Status == OrderStatus.Shipped || Status == OrderStatus.Paid)
      throw new DomainException("Pedidos pagos ou enviados não podem ser cancelados diretamente.");

    Status = OrderStatus.Cancelled;
    AddDomainEvent(new OrderCancelledDomainEvent(Id));
  }
}