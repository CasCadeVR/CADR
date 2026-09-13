using System.Diagnostics.CodeAnalysis;
using CADR.Administrations.Entities.Configurations;
using CADR.Adrs.Entities.Configurations;
using CADR.Context.Contracts;
using CADR.Context.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CADR.Context;

/// <summary>
/// Контекст базы данных Cadr
/// </summary>
public class CadrContext : DbContext,
    IReader,
    IWriter,
    IUnitOfWork
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrContext"/>
    /// </summary>
    public CadrContext([NotNull] DbContextOptions<CadrContext> options)
        : base(options)
    {
        // https://support.aspnetzero.com/QA/Questions/11011/Cannot-write-DateTime-with-KindLocal-to-PostgreSQL-type-%27timestamp-with-time-zone%27-only-UTC-is-supported
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyAllConfigurations(typeof(AdministrationEntitiesAnchor).Assembly);
        modelBuilder.ApplyAllConfigurations(typeof(AdrsEntitiesAnchor).Assembly);
    }

    IQueryable<TEntity> IReader.Read<TEntity>()
        => base.Set<TEntity>()
            .AsNoTracking()
            .AsQueryable();

    void IWriter.Add<TEntity>([NotNull] TEntity entity)
        => base.Entry(entity).State = EntityState.Added;

    void IWriter.Update<TEntity>([NotNull] TEntity entity)
        => base.Entry(entity).State = EntityState.Modified;

    void IWriter.Delete<TEntity>([NotNull] TEntity entity)
        => base.Entry(entity).State = EntityState.Deleted;

    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        var count = await base.SaveChangesAsync(cancellationToken);
        foreach (var entry in base.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return count;
    }
}
