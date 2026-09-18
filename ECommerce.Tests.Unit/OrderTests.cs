using FluentAssertions;
using ECommerce.Domain.Orders;
using ECommerce.Domain.ValueObjects;
using ECommerce.Domain.Exceptions;
using Xunit;

namespace ECommerce.Tests.Unit.Domain;

public class OrderTests
{
  [Fact]
  public void CreateOrder_Should_SetInitialStateToPending_And_RaiseDomainEvent()
  {
    // Arrange
    var customerId = Guid.NewGuid();

    // Act
    var order = new Order(customerId);

    // Assert
    order.CustomerId.Should().Be(customerId);
    order.Status.Should().Be(OrderStatus.Pending);
    order.Items.Should().BeEmpty();
    order.DomainEvents.Should().ContainSingle(e => e is OrderCreatedDomainEvent);
  }

  [Fact]
  public void AddItem_Should_AddNewItemAndCalculateTotal_When_OrderIsPending()
  {
    // Arrange
    var order = new Order(Guid.NewGuid());
    var productId = Guid.NewGuid();
    var unitPrice = new Money(100m, "BRL");

    // Act
    order.AddItem(productId, "Teclado Mecânico", unitPrice, 2);

    // Assert
    order.Items.Should().HaveCount(1);
    order.TotalAmount.Amount.Should().Be(200m);
    order.TotalAmount.Currency.Should().Be("BRL");
  }

  [Fact]
  public void AddItem_Should_ThrowDomainException_When_OrderIsNotPending()
  {
    // Arrange
    var order = new Order(Guid.NewGuid());
    order.AddItem(Guid.NewGuid(), "Mouse", new Money(50m), 1);
    order.MarkAsPaid(); // O pedido agora está pago, alterando o status

    // Act
    // Tentamos adicionar um item após o pedido já estar pago
    Action act = () => order.AddItem(Guid.NewGuid(), "Mousepad", new Money(30m), 1);

    // Assert
    act.Should().Throw<DomainException>()
       .WithMessage("Não é possível alterar um pedido que não está pendente.");
  }

  [Fact]
  public void MarkAsPaid_Should_ChangeStatusToPaid_And_RaiseEvent_When_OrderHasItems()
  {
    // Arrange
    var order = new Order(Guid.NewGuid());
    order.AddItem(Guid.NewGuid(), "Headset", new Money(250m), 1);

    // Act
    order.MarkAsPaid();

    // Assert
    order.Status.Should().Be(OrderStatus.Paid);
    order.DomainEvents.Should().Contain(e => e is OrderPaidDomainEvent);
  }

  [Fact]
  public void MarkAsPaid_Should_ThrowDomainException_When_OrderHasNoItems()
  {
    // Arrange
    var order = new Order(Guid.NewGuid());

    // Act
    Action act = () => order.MarkAsPaid();

    // Assert
    act.Should().Throw<DomainException>()
       .WithMessage("Não é possível pagar um pedido sem itens.");
  }
}