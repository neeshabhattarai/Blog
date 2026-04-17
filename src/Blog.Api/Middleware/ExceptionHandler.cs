using Microsoft.AspNetCore.Diagnostics;

namespace Blog.Middleware;

public class ExceptionHandler:IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        { 
             next(context);
             return Task.CompletedTask;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            context.Response.WriteAsync(e.Message);
            return Task.CompletedTask;
        }
    }
    
}