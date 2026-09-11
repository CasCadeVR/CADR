using CADR.Common.Core.Contracts;

namespace CADR.Common.Core.Implementations;

/// <inheritdoc cref="IDateTimeProvider"/>
public class DateTimeProvider : IDateTimeProvider
{
    DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;
}
