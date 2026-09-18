namespace ECommerce.Application.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
  public CreateOrderCommandValidator()
  {
    RuleFor(x => x.CustomerId)
        .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

    RuleFor(x => x.Items)
        .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

    RuleForEach(x => x.Items).ChildRules(item =>
    {
      item.RuleFor(i => i.ProductId)
              .NotEmpty().WithMessage("O ID do produto é obrigatório.");

      item.RuleFor(i => i.Quantity)
              .GreaterThan(0).WithMessage("A quantidade do item deve ser maior que zero.");

      item.RuleFor(i => i.UnitPrice)
              .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
    });
  }
}