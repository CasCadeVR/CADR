using CADR.Common.Core.Contracts.Models;

namespace CADR.Common.Mvc.Models;

/// <summary>
/// Информация об ошибках валидации работы АПИ
/// </summary>
public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = [];
}
