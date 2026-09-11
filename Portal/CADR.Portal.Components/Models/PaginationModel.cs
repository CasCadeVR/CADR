namespace CADR.Portal.Components.Models;

/// <summary>
/// Модель с данными пагинации
/// </summary>
public struct PaginationModel
{
    private int pageNum = 1;
    private int pageSize = 20;

    /// <summary>
    /// Инициализирует экземпляр <see cref="PaginationModel"/>
    /// </summary>
    public PaginationModel() { }

    /// <summary>
    /// Размер страницы
    /// </summary>
    public int PageSize
    {
        readonly get => pageSize;
        set => pageSize = Math.Max(value, 1);
    }

    /// <summary>
    /// Номер страницы
    /// </summary>
    public int PageNum
    {
        readonly get => pageNum;
        set => pageNum = Math.Max(value, 1);
    }
}
