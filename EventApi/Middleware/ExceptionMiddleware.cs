using Event.Model;
using Event.Model.Exception;

namespace Event.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        HttpStatus status;
        string message;
        try
        {
            await next(context);
            return;
        }
        catch (HttpException e)
        {
            status = e.HttpStatus;
            message = e.Message;
        }
        catch (NotImplementedException e)
        {
            status = HttpStatus.NotImplemented;
            message = e.Message;
        }
        catch (Exception e)
        {
            status = HttpStatus.InternalError;
            message = e.Message;
        }

        context.Response.StatusCode = (int) status;
        await context.Response.WriteAsync(message);
    }
}