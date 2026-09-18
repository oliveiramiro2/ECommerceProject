using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Orders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.ToTable("Orders");

    builder.HasKey(o => o.Id);

    builder.Property(o => o.CustomerId)
        .IsRequired();

    builder.Property(o => o.Status)
        .IsRequired()
        .HasConversion<int>(); // Salva o Enum como inteiro no banco

    // 🛡️ CONCORRÊNCIA OTIMISTA: Diz ao EF Core que esta coluna monitora conflitos
    builder.Property(o => o.Version)
        .IsConcurrencyToken();

    // Mapeamento do relacionamento com a entidade filha OrderItem
    builder.HasMany(o => o.Items)
        .WithOne()
        .HasForeignKey(i => i.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

    // Informa ao EF Core para acessar a lista privada _items através do campo de apoio
    var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
    navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}