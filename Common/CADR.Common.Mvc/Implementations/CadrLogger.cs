using CADR.Common.Core.Contracts;
using Serilog;
using Serilog.Events;

namespace CADR.Common.Mvc.Implementations;

/// <inheritdoc />
internal class CadrLogger : ICadrLogger
{
    private readonly ILogger logger;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="CadrLogger"/>
    /// </summary>
    private CadrLogger(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Создаёт новый экземпляр <see cref="ICadrLogger"/>
    /// </summary>
    static internal ICadrLogger Create(ILogger logger)
        => new CadrLogger(logger);

    void ICadrLogger.Trace(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Verbose, null, messageTemplate, propertyValues);

    void ICadrLogger.Trace(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Verbose, exception, messageTemplate, propertyValues);

    void ICadrLogger.Debug(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Debug, null, messageTemplate, propertyValues);

    void ICadrLogger.Debug(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Debug, exception, messageTemplate, propertyValues);

    void ICadrLogger.Info(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Information, null, messageTemplate, propertyValues);

    void ICadrLogger.Info(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Information, exception, messageTemplate, propertyValues);

    void ICadrLogger.Warn(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Warning, null, messageTemplate, propertyValues);

    void ICadrLogger.Warn(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Warning, exception, messageTemplate, propertyValues);

    void ICadrLogger.Error(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Error, null, messageTemplate, propertyValues);

    void ICadrLogger.Error(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Error, exception, messageTemplate, propertyValues);

    void ICadrLogger.Fatal(string messageTemplate, params object[] propertyValues)
        => Write(LogEventLevel.Fatal, null, messageTemplate, propertyValues);

    void ICadrLogger.Fatal(string messageTemplate, Exception exception, params object[] propertyValues)
        => Write(LogEventLevel.Fatal, exception, messageTemplate, propertyValues);

    private void Write(LogEventLevel level, Exception? exception, string messageTemplate, object[] propertyValues)
        => logger.Write(level, exception, messageTemplate, propertyValues);
}
