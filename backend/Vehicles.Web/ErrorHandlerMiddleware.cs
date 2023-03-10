using Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace Vehicles.Web;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception error)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case EntityNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case PageNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case InvalidDateFilterException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                   
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = $"{error?.Message} Status code: {response.StatusCode}" });
            await response.WriteAsync(result);
        }
    }
}