using JobsityChallenge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        // Apply migrations automatically in Development
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}
