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
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            await RenameOperatorRoleAsync(dbContext, roleManager);

            foreach (var roleName in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    EnsureIdentityResultSucceeded(result, roleName);
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

    private static async Task RenameOperatorRoleAsync(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
    {
        const string legacyRoleName = "Operador";

        var legacyRole = await roleManager.FindByNameAsync(legacyRoleName);
        var sellerRole = await roleManager.FindByNameAsync(AppRoles.Vendedor);

        if (legacyRole is null)
        {
            return;
        }

        if (sellerRole is null)
        {
            legacyRole.Name = AppRoles.Vendedor;
            legacyRole.NormalizedName = roleManager.NormalizeKey(AppRoles.Vendedor);
            var result = await roleManager.UpdateAsync(legacyRole);
            EnsureIdentityResultSucceeded(result, AppRoles.Vendedor);
            return;
        }

        if (legacyRole.Id == sellerRole.Id)
        {
            return;
        }

        var sellerUserIds = await dbContext.UserRoles
            .Where(userRole => userRole.RoleId == sellerRole.Id)
            .Select(userRole => userRole.UserId)
            .ToListAsync();
        var sellerUserIdSet = sellerUserIds.ToHashSet();

        var legacyAssignments = await dbContext.UserRoles
            .Where(userRole => userRole.RoleId == legacyRole.Id)
            .ToListAsync();

        foreach (var assignment in legacyAssignments)
        {
            if (!sellerUserIdSet.Contains(assignment.UserId))
            {
                dbContext.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = assignment.UserId,
                    RoleId = sellerRole.Id
                });
            }

            dbContext.UserRoles.Remove(assignment);
        }

        await dbContext.SaveChangesAsync();

        var deleteResult = await roleManager.DeleteAsync(legacyRole);
        EnsureIdentityResultSucceeded(deleteResult, legacyRoleName);
    }

    private static void EnsureIdentityResultSucceeded(IdentityResult result, string roleName)
    {
        if (result.Succeeded)
        {
            return;
        }

        var errors = string.Join(", ", result.Errors.Select(error => error.Description));
        throw new InvalidOperationException($"Nao foi possivel configurar a role '{roleName}': {errors}");
    }
}
