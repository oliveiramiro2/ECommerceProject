using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Common.Behaviors;

namespace ECommerce.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    var assembly = typeof(DependencyInjection).Assembly;

    // Registra o MediatR
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    // Registra todos os validadores do FluentValidation automaticamente
    services.AddValidatorsFromAssembly(assembly);

    // Registra o Behavior de Validação no pipeline do MediatR
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    return services;
  }
}