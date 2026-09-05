using Microsoft.EntityFrameworkCore;

namespace Cadr.Context;

/// <summary>
/// Фабрика для контекста <see cref="CadrContext"/>
/// </summary>
public class CadrContextFactory
{
    /// <summary>
    /// Выполняет миграцию базы данных <see cref="CadrContext"/>
    /// </summary>
    public static async Task Migrate(string[] args)
    {
        await using var context = new CadrDesignTimeContextFactory().CreateDbContext(args);
        await context.Database.MigrateAsync();
    }
}
