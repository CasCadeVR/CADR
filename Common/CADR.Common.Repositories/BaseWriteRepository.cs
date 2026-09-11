using System.Diagnostics.CodeAnalysis;
using CADR.Common.Repositories.Contracts;
using CADR.Context.Contracts;
using CADR.Context.Entities.Contracts.Interfaces;

namespace CADR.Common.Repositories;

/// <summary>
/// Базовый класс репозитория записи данных
/// </summary>
public abstract class BaseWriteRepository<T> : IDbWriter<T> where T : class, IEntity
{
    /// <inheritdoc cref="IDbWriterContext"/>
    private readonly IDbWriterContext writerContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BaseWriteRepository{T}"/>
    /// </summary>
    protected BaseWriteRepository(IDbWriterContext writerContext)
    {
        this.writerContext = writerContext;
    }

    /// <inheritdoc cref="IDbWriter{T}"/>
    public virtual void Add([NotNull] T entity)
    {
        if (entity is IEntityWithId entityWithId &&
            entityWithId.Id == Guid.Empty)
        {
            entityWithId.Id = Guid.NewGuid();
        }
        AuditForCreate(entity);
        AuditForUpdate(entity);
        writerContext.Writer.Add(entity);
    }

    /// <inheritdoc cref="IDbWriter{T}"/>
    public void Update([NotNull] T entity)
    {
        AuditForUpdate(entity);
        writerContext.Writer.Update(entity);
    }

    /// <inheritdoc cref="IDbWriter{T}"/>
    public void Delete([NotNull] T entity)
    {
        AuditForUpdate(entity);
        AuditForDelete(entity);
        if (entity is IEntityAuditDeletedAt)
        {
            writerContext.Writer.Update(entity);
        }
        else
        {
            writerContext.Writer.Delete(entity);
        }
    }

    private void AuditForCreate([NotNull] T entity)
    {
        if (entity is IEntityAuditCreated auditCreated)
        {
            auditCreated.CreatedAt = writerContext.DateTimeProvider.UtcNow;
            auditCreated.CreatedBy = writerContext.IdentityProvider.Name;
        }
    }

    private void AuditForUpdate([NotNull] T entity)
    {
        if (entity is IEntityAuditUpdate auditUpdate)
        {
            auditUpdate.UpdatedAt = writerContext.DateTimeProvider.UtcNow;
            auditUpdate.UpdatedBy = writerContext.IdentityProvider.Name;
        }
    }

    private void AuditForDelete([NotNull] T entity)
    {
        if (entity is IEntityAuditDeletedAt auditDeleted)
        {
            auditDeleted.DeletedAt = writerContext.DateTimeProvider.UtcNow;
        }
    }
}
