using MediatR;

namespace ECommerce.Application.Orders.Commands;

// O Command que o MediatR vai processar, retornando o ID do pedido criado (Guid)
public record CreateOrderCommand(
    Guid CustomerId,
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;