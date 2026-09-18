namespace ECommerce.Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
  private readonly IOrderRepository _orderRepository;

  public CreateOrderCommandHandler(IOrderRepository orderRepository)
  {
    _orderRepository = orderRepository;
  }

  public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    // 1. Instancia o Agregado raiz protegido pelas regras do DDD
    var order = new Order(request.CustomerId);

    // 2. Adiciona os itens utilizando os métodos de negócio do agregado
    foreach (var itemDto in request.Items)
    {
      var unitPrice = new Money(itemDto.UnitPrice);
      order.AddItem(itemDto.ProductId, itemDto.ProductName, unitPrice, itemDto.Quantity);
    }

    // 3. Persiste utilizando o repositório
    await _orderRepository.AddAsync(order, cancellationToken);

    // 4. Retorna o ID gerado
    return order.Id;
  }
}