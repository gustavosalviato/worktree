using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkTree.Infra.DataAccess;

namespace WorkTree.Infra.Migrations;

public class DatabaseMigration
{
    public static async Task ExecuteMigrations(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<WorkTreeDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}