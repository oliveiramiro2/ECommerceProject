using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Orders;
using ECommerce.Domain.Orders;

namespace ECommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
  private readonly ECommerceDbContext _context;

  public OrderRepository(ECommerceDbContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Order order, CancellationToken cancellationToken)
  {
    await _context.Orders.AddAsync(order, cancellationToken);

    try
    {
      await _context.SaveChangesAsync(cancellationToken);
    }
    catch (DbUpdateConcurrencyException)
    {
      // Conflito de concorrência otimista detectado!
      throw new InvalidOperationException("O registro foi modificado ou esgotado por outra transação simultânea. Tente novamente.");
    }
  }
}