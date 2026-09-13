using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Migrations;

public sealed class DatabaseMigrator(IServiceProvider serviceProvider, ILogger<DatabaseMigrator> logger)
{
    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        var migrations = serviceProvider.GetServices<IDatabaseMigration>();

        foreach (var migration in migrations)
        {
            var name = migration.GetType().Name;
            logger.LogInformation("Applying database migration {Migration}", name);

            await migration.ApplyAsync(cancellationToken);
        }
    }
}
