using DemoApi.Services;
using Microsoft.AspNetCore.Http;

namespace DemoApi.Middleware
{
    public class JwtValidator
    {
        private readonly RequestDelegate _next;

        public JwtValidator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            if (context.Request.Path.Equals("/api/account/login") || context.Request.Path.Equals("/api/account/register"))
            {
                await _next(context);
                return;
            }

            var username = context.User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;

            if (string.IsNullOrEmpty(username) || !await userService.CheckUserExists(username))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            await _next(context);
        }
    }
}
