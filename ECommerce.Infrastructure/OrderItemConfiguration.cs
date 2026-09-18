using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Orders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.ToTable("OrderItems");

    builder.HasKey(i => i.Id);

    builder.Property(i => i.ProductId)
        .IsRequired();

    builder.Property(i => i.ProductName)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(i => i.Quantity)
        .IsRequired();

    // Mapeando o Value Object Money embutido na tabela do item
    builder.OwnsOne(i => i.UnitPrice, price =>
    {
      price.Property(m => m.Amount).HasColumnName("UnitPrice_Amount").HasColumnType("decimal(18,2)");
      price.Property(m => m.Currency).HasColumnName("UnitPrice_Currency").HasMaxLength(3);
    });
  }
}