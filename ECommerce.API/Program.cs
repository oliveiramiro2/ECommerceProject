using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.API.Endpoints;
using ECommerce.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona serviços ao container de DI do ASP.NET Core
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Registra as camadas de Aplicação e Infraestrutura através dos nossos métodos de extensão
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// 3. Configura o pipeline HTTP
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. Mapeia os Endpoints da API
app.MapOrderEndpoints();

app.Run();

// Necessário para o Testcontainers / WebApplicationFactory enxergar a API nos Testes de Integração
public partial class Program { }