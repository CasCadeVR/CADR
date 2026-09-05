namespace CADR.Common.Core.Contracts;

/// <summary>
/// Осуществляет логирование в сконфигурированный логгер
/// </summary>
public interface ICadrLogger
{
    /// <summary>
    /// Логирует с уровнем <see langword="Trace"/>
    /// </summary>
    void Trace(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Trace"/> и заданным исключением
    /// </summary>
    void Trace(string messageTemplate, Exception exception, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Debug"/>
    /// </summary>
    void Debug(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Debug"/> и заданным исключением
    /// </summary>
    void Debug(string messageTemplate, Exception exception, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Info"/>
    /// </summary>
    void Info(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Info"/> и заданным исключением
    /// </summary>
    void Info(string messageTemplate, Exception exception, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Warn"/>
    /// </summary>
    void Warn(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Warn"/> и заданным исключением
    /// </summary>
    void Warn(string messageTemplate, Exception exception, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Error"/>
    /// </summary>
    void Error(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Error"/> и заданным исключением
    /// </summary>
    void Error(string messageTemplate, Exception exception, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Fatal"/>
    /// </summary>
    void Fatal(string messageTemplate, params object[] propertyValues);

    /// <summary>
    /// Логирует с уровнем <see langword="Fatal"/> и заданным исключением
    /// </summary>
    void Fatal(string messageTemplate, Exception exception, params object[] propertyValues);
}
