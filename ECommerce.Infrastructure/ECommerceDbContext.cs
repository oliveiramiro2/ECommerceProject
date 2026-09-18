using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Orders;

namespace ECommerce.Infrastructure.Persistence;

public class ECommerceDbContext : DbContext
{
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<OrderItem> OrderItems => Set<OrderItem>();

  public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Aplica automaticamente todas as classes que implementam IEntityTypeConfiguration neste assembly
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
  }
}