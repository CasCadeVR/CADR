using CADR.Api.Client;
using CADR.Portal.Components.Enums;

namespace CADR.Portal.Components.Models
{
    /// <summary>
    /// Модель предмета в папке (папка или файл (в нашем случае ADR))
    /// </summary>
    public sealed record ExplorerItem(Guid? AdrId, Guid? FolderId, string Caption, string Url, IconTypes? Icon, int? Number, AdrStatusApi? Status, DateTimeOffset? UpdatedAt);
}
