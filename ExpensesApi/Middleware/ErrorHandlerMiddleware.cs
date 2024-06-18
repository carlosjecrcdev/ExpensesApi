
using ExpensesApi.Exceptions;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ExpensesApi.Middleware
{
    /// <summary>
    /// Middleware for handling errors in http request
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlerMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware for handle http context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context,error);
            }
        }

        /// <summary>
        /// Handles error status codes 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            _logger.LogError(error, "An unexpected error occurred.");

            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case ApiExceptions e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest; break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound; break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; break;
            }
            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
