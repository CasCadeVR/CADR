namespace CADR.Common.Core.Contracts;

/// <summary>
/// Менеджер по созданию логгера
/// </summary>
public interface ICadrLogManager
{
    /// <summary>
    /// Создаёт логгер для указанного типа
    /// </summary>
    ICadrLogger GetLogger<T>();

    /// <summary>
    /// Создаёт логгер для указанноого типа и переданными тегами
    /// </summary>
    ICadrLogger GetLogger<T>(params string[] tags);
}
