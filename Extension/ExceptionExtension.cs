using Event.Middleware;

namespace Event.Extension;

public static class ExceptionExtension
{
    public static void AddExceptionHandling(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}