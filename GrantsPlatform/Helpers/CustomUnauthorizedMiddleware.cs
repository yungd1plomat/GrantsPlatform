using GrantsPlatform.Models.Viewmodels;
using System.Text.Json;

namespace GrantsPlatform.Helpers
{
    public class CustomUnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomUnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorResponse
                {
                    Code = StatusCodes.Status401Unauthorized,
                    Message = "Unauthorized"
                };
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }

}
