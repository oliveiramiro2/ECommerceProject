using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;
using ECommerce.Infrastructure.Persistence;

namespace ECommerce.Tests.Integration;

public class ECommerceWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  // Configura o container do PostgreSQL utilizando uma imagem oficial leve
  private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
      .WithImage("postgres:15-alpine")
      .WithDatabase("ecommerce_test_db")
      .WithUsername("postgres")
      .WithPassword("postgres")
      .Build();

  // Executado ANTES de rodar os testes da suíte
  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      // 1. Remove a configuração padrão do DbContext da API (que aponta para o banco de produção/desenvolvimento)
      var descriptor = services.SingleOrDefault(
              d => d.ServiceType == typeof(DbContextOptions<ECommerceDbContext>));

      if (descriptor != null)
      {
        services.Remove(descriptor);
      }

      // 2. Registra o DbContext apontando para a string de conexão do PostgreSQL do Testcontainers
      services.AddDbContext<ECommerceDbContext>(options =>
          {
          options.UseNpgsql(_dbContainer.GetConnectionString());
        });

      // 3. Garante que o banco e as tabelas/migrações sejam criados no container recém-subido
      using var scope = services.BuildServiceProvider().CreateScope();
      var scopedServices = scope.ServiceProvider;
      var db = scopedServices.GetRequiredService<ECommerceDbContext>();

      db.Database.EnsureCreated();
    });
  }

  // Executado DEPOIS que todos os testes terminam (destrói o container Docker)
  public new async Task DisposeAsync()
  {
    await _dbContainer.StopAsync();
    await _dbContainer.DisposeAsync();
  }
}