using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using System.Text.Json;

namespace ECommerce.Tests.Integration;

public class OrdersIntegrationTests : IClassFixture<ECommerceWebApplicationFactory>
{
  private readonly HttpClient _client;

  public OrdersIntegrationTests(ECommerceWebApplicationFactory factory)
  {
    // Cria um cliente HTTP apontando para a nossa API em memória integrada com o Docker
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task CreateOrderEndpoint_Should_ReturnCreatedId_When_CommandIsValid()
  {
    // Arrange
    var command = new
    {
      CustomerId = Guid.NewGuid(),
      Items = new[]
        {
                new
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Teclado Mecânico RGB",
                    UnitPrice = 350.0m,
                    Quantity = 1
                }
            }
    };

    // Act
    // Faz uma requisição POST real para o endpoint da API (que criaremos na camada API)
    var response = await _client.PostAsJsonAsync("/api/orders", command);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);

    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonDocument>();
    var orderId = jsonResponse!.RootElement.GetProperty("orderId").GetGuid();
    orderId.Should().NotBeEmpty();
  }
}