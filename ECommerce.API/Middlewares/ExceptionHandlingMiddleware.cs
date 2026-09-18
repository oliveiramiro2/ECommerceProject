using System.Net;
using System.Text.Json;
using ECommerce.Domain.Exceptions;
using FluentValidation;

namespace ECommerce.API.Middlewares;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Ocorreu um erro não tratado durante a requisição.");
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    var statusCode = HttpStatusCode.InternalServerError;
    object response = new { error = "Ocorreu um erro interno no servidor." };

    switch (exception)
    {
      case ValidationException validationEx:
        statusCode = HttpStatusCode.BadRequest;
        response = new { errors = validationEx.Errors.Select(e => e.ErrorMessage) };
        break;

      case DomainException domainEx:
        statusCode = HttpStatusCode.BadRequest;
        response = new { error = domainEx.Message };
        break;

      case InvalidOperationException invOpEx when invOpEx.Message.Contains("concorrência"):
        // Retorna 409 Conflict quando houver concorrência otimista no estoque/pedido!
        statusCode = HttpStatusCode.Conflict;
        response = new { error = invOpEx.Message };
        break;
    }

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)statusCode;

    var json = JsonSerializer.Serialize(response);
    await context.Response.WriteAsync(json);
  }
}