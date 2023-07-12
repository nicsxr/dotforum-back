namespace DotForum.API.Middlewares;

public class AuthenticationStatusMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationStatusMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the user is authenticated
        bool isAuthenticated = context.User.Identity.IsAuthenticated;

        // Add or modify the header based on authentication status
        context.Response.Headers.Add("X-Is-Authenticated", isAuthenticated.ToString());

        // Call the next middleware in the pipeline
        await _next(context);
    }
}