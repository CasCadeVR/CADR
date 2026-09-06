using CADR.Context;

namespace CADR.Api.Infrastructures;

public sealed partial class CadrConfiguration : ICadrContextConfiguration
{
    string ICadrContextConfiguration.ConnectionString
        => configuration.GetConnectionString("CadrConnection")!;
}
