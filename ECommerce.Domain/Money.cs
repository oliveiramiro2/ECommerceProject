namespace ECommerce.Domain.ValueObjects;

public record Money
{
  public decimal Amount { get; }
  public string Currency { get; }

  public Money(decimal amount, string currency = "BRL")
  {
    if (amount < 0)
      throw new DomainException("O valor monetário não pode ser negativo.");

    Amount = amount;
    currency = currency.Trim().ToUpper();
    Currency = string.IsNullOrWhiteSpace(currency) ? "BRL" : currency;
  }

  public static Money operator +(Money a, Money b)
  {
    if (a.Currency != b.Currency)
      throw new DomainException("Não é possível somar moedas de tipos diferentes.");
    return new Money(a.Amount + b.Amount, a.Currency);
  }

  public static Money Zero(string currency = "BRL") => new(0m, currency);
}