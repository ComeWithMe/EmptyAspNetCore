using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _anystr;

    public TokenMiddleware(RequestDelegate next,string anystr)
    {
        this._anystr = anystr;
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Query["token"];
        if (_anystr != "12345678")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Token is invalid");
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}

public static class TokenExtensions
{
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string anystr)
    {
        return builder.UseMiddleware<TokenMiddleware>(anystr);
    }
}