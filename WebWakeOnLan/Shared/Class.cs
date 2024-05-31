using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string Realm = "My Realm";

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Realm}\"";
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
        var username = credentials[0];
        var password = credentials[1];

        if (!IsAuthorized(username, password))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }

    public bool IsAuthorized(string username, string password)
    {
        // Zde byste měli přidat vaši logiku pro ověření uživatele
        return username == "admin" && password == "password"; // příklad
    }
}

// Rozšíření pro middleware
public static class BasicAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BasicAuthMiddleware>();
    }
}
