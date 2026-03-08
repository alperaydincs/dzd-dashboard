using DZDDashboard.Api.Middleware;

namespace DZDDashboard.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseMiddleware<EntraUserSyncMiddleware>();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
