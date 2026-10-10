namespace CADR.Adrs.Repositories.Contracts.Models.Adrs;

/// <summary>
/// Результат создания Adr
/// </summary>
public class CreateAdrResult
{
    /// <summary>
    /// Полученный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полученный номер
    /// </summary>
    public int Number { get; set; }
}
