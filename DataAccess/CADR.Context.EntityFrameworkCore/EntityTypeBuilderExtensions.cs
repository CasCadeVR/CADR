using CADR.Context.Entities.Contracts.Interfaces;
using CADR.Context.Entities.Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Context.EntityFrameworkCore;

/// <summary>
/// Методы расширения для <see cref="EntityTypeBuilder"/>
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Задаёт конфигурацию свойст аудита добавления для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void CreateOnlyAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditCreated
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
    }

    /// <summary>
    /// Задаёт конфигурацию свойст полного аудита для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void PropertyAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : BaseAuditEntity
    {
        builder.CreateOnlyAuditConfiguration();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.UpdatedBy).IsRequired().HasMaxLength(200);
    }

    /// <summary>
    /// Задаёт конфигурацию ключа для идентификатора <see cref="IEntityWithId.Id"/>
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);
}
