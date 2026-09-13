namespace BuildingBlocks.Migrations;

public interface IDatabaseMigration
{
    Task ApplyAsync(CancellationToken cancellationToken = default);
}
