namespace ECommerce.Application.Orders;

public interface IOrderRepository
{
  Task AddAsync(Order order, CancellationToken cancellationToken);
}