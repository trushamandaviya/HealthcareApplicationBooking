using System.Net;
using Newtonsoft.Json;

namespace HC.API.Middleware
{
    public class GenericExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GenericExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Default to 500

            // Customize status codes based on exception types
            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest; // 400
            }
            else if (exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.Conflict; // 409
            }

            var response = new
            {
                Error = exception.Message,
                Details = exception.StackTrace // Omit in production
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}

