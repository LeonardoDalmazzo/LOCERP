using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Locerp.Web.Authorization;

namespace Locerp.Web.Data;

public static class RoleSeedExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    public static async Task SeedRolesAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            foreach (var roleName in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        catch (PostgresException exception) when (exception.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            throw new InvalidOperationException(
                "The Identity tables do not exist yet. Apply the EF Core migrations before seeding roles, or set 'Database:ApplyMigrationsOnStartup' to true in development.",
                exception);
        }
    }
}
