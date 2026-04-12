using System.Net;

namespace ProductService.Api.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomExeptionHandllingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExeptionHandllingMiddleware> _logger;

        public CustomExeptionHandllingMiddleware(RequestDelegate next, ILogger<CustomExeptionHandllingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred while processing the request.");

                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, "Inner exception details:");
                }

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Message = ex.Message,
                    Type = ex.GetType().ToString()
                });

            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomExeptionHandllingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExeptionHandllingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExeptionHandllingMiddleware>();
        }
    }
}
