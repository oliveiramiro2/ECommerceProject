using MediatR;
using ECommerce.Application.Orders.Commands;

namespace ECommerce.API.Endpoints;

public static class OrderEndpoints
{
  public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapPost("/api/orders", async (CreateOrderCommand command, ISender sender, CancellationToken cancellationToken) =>
    {
      // O MediatR despacha o comando para o Handler de Aplicação correspondente
      var orderId = await sender.Send(command, cancellationToken);

      // Retorna 201 Created apontando para a URI do recurso criado
      return Results.Created($"/api/orders/{orderId}", new { orderId });
    })
    .WithName("CreateOrder")
    .Produces<Guid>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict)
    .WithTags("Orders");

    return app;
  }
}