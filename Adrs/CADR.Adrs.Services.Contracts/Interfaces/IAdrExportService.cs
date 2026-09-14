namespace CADR.Adrs.Services.Contracts.Interfaces;

/// <summary>
/// Экспорт ADR в Markdown (MADR)
/// </summary>
public interface IAdrExportService
{
    /// <summary>
    /// Экспортирует ADR в Markdown (MADR-подобный формат)
    /// </summary>
    Task<string> ExportToMarkdownAsync(Guid adrId, Guid userId, CancellationToken cancellationToken);
}
