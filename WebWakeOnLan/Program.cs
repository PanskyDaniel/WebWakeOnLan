using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using WebWakeOnLan.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(x => x.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

builder.Services.AddSession();

builder.Services.AddAuthorization();

builder.Services.AddSingleton<Wake>();

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddFile(o => o.RootPath = builder.Environment.ContentRootPath);

var app = builder.Build();

//app.UseStaticFiles();

app.UseSession();

app.UseMvc();
app.UseRouting();

app.UseAuthorization();



app.Use(async (context, next) =>
{
    var isAuthenticated = context.Session.GetString("IsAuthenticated");

    if (string.IsNullOrEmpty(isAuthenticated) || isAuthenticated != "true")
    {
        string authHeader = context.Request.Headers["Authorization"];
        if (authHeader != null && authHeader.StartsWith("Basic"))
        {
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int separatorIndex = usernamePassword.IndexOf(':');
            var username = usernamePassword.Substring(0, separatorIndex);
            var password = usernamePassword.Substring(separatorIndex + 1);

            if (username == "VIS" && password == "VIS")
            {
                context.Session.SetString("IsAuthenticated", "true");
                context.Response.Redirect("/wake");
                return;
            }
        }

        context.Response.Headers["WWW-Authenticate"] = "Basic";
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }

    await next.Invoke();
});

app.MapGet("/wake", async (HttpContext context) =>
{
    context.Response.Redirect("/wake/index");
    return Results.Ok("Pøesmìrováno na Wake-on-LAN.");
}).RequireAuthorization();

app.MapGet("/wake/{mac}", async (string mac, Wake wakeService) =>
{
    wakeService.PoslatWakeOnLan(mac);
    return Results.Ok("paket se poslal");
}).RequireAuthorization();

app.Run();
