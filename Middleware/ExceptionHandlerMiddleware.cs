using System.Net;
using Pix.Exceptions;

namespace Pix.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
  private readonly RequestDelegate _next = next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    _logger.LogError(exception, "An unexpected error occurred.");

    //More log stuff        

    ExceptionResponse response = exception switch
    {
      RabbitMqException _ => new ExceptionResponse(HttpStatusCode.InternalServerError, exception.Message),
      RecentPaymentException _ => new ExceptionResponse(HttpStatusCode.Conflict, exception.Message),
      NotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
      TokenInvalidException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message),
      MaximumKeysException _ => new ExceptionResponse(HttpStatusCode.UnprocessableEntity, exception.Message),
      InvalidKeyValueException _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
      ExistingKeyException _ => new ExceptionResponse(HttpStatusCode.Conflict, exception.Message),
      TypeNotMatchException _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
      AccountBadRequestException _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
      _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsJsonAsync(response);
  }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);