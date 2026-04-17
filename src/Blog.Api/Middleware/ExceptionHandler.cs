using Microsoft.AspNetCore.Diagnostics;

namespace Blog.Middleware;

public class ExceptionHandler:IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
    }
    
}