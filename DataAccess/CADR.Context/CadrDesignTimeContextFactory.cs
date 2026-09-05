using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cadr.Context;

/// <summary>
/// Фабрика для создания контекста в DesignTime
/// </summary>
public class CadrDesignTimeContextFactory : IDesignTimeDbContextFactory<CadrContext>
{
    /// <summary>
    /// Название параметра строки подключения для мигрирования базы данных
    /// </summary>
    public const string MigrateDatabaseKey = "--migrate-context";

    /// <summary>
    /// Creates a new instance of a derived context
    /// </summary>
    /// <remarks>
    /// 1) dotnet tool install --global dotnet-ef
    /// 2) dotnet tool update --global dotnet-ef
    /// 3) dotnet ef migrations add [name] --project DataAccess\CADR.Context\CADR.Context.csproj
    /// 4) dotnet ef database update --project DataAccess\CADR.Context\CADR.Context.csproj
    /// 5) dotnet ef database update [targetMigrationName] --project DataAccess\CADR.Context\CADR.Context.csproj
    /// </remarks>
    public CadrContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Port=5432;Database=cadr;Username=postgres;Password=Qwerty123456!";
        if (args.Length > 1)
        {
            var index = Array.FindIndex(args, row => row.Contains(MigrateDatabaseKey, StringComparison.InvariantCulture));
            if (index != -1 && index + 1 < args.Length)
            {
                connectionString = args[index + 1];
            }
        }
        var options = new DbContextOptionsBuilder<CadrContext>()
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine)
            .Options;

        return new CadrContext(options);
    }
}
