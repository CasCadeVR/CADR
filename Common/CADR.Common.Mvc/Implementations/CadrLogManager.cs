using CADR.Common.Core.Contracts;
using Serilog;

namespace CADR.Common.Mvc.Implementations;

/// <inheritdoc />
public class CadrLogManager : ICadrLogManager
{
    private readonly ILogger rootLogger;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrLogManager"/>
    /// </summary>
    public CadrLogManager(ILogger rootLogger)
    {
        this.rootLogger = rootLogger;
    }

    ICadrLogger ICadrLogManager.GetLogger<T>()
        => CadrLogger.Create(GetLoggerInternal(typeof(T).FullName, []));

    ICadrLogger ICadrLogManager.GetLogger<T>(params string[] tags)
        => CadrLogger.Create(GetLoggerInternal(typeof(T).FullName, tags));

    private ILogger GetLoggerInternal(string? loggerName, IEnumerable<string> tags)
    {
        var result = rootLogger.ForContext("Logger", loggerName ?? "unknown");

        if (tags.Any())
        {
            result = result.ForContext("Tags", string.Join(", ", tags));
        }

        return result;
    }
}
