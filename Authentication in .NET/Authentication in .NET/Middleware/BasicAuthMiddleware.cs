using System.Text;

namespace Day16BasicAuth.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader =
                context.Request.Headers["Authorization"];

            if (authHeader != null &&
               authHeader.StartsWith("Basic "))
            {
                var encodedValue =
                    authHeader.Substring(6);

                var decodedBytes =
                    Convert.FromBase64String(encodedValue);

                var decodedValue =
                    Encoding.UTF8.GetString(decodedBytes);

                var credentials =
                    decodedValue.Split(':');

                string username = credentials[0];
                string password = credentials[1];

                if (username == "hemant" &&
                   password == "123")
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = 401;

            await context.Response.WriteAsync(
                "Unauthorized");
        }
    }
}