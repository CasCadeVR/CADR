using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CADR.Administrations.Pages.Infrastructures;
using Xunit;

namespace CADR.Portal.Tests;

/// <summary>
/// Тесты профилей автомаппера
/// </summary>
public class AutoMapperProfileTests
{
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AutoMapperProfileTests"/>
    /// </summary>
    public AutoMapperProfileTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.EnableEnumMappingValidation();
            opts.AddProfile<AdministrationsProfile>();
        });

        mapper = config.CreateMapper();
    }

    /// <summary>
    /// Маппинг правильно сформирован
    /// </summary>
    [Fact]
    public void ValidateMapperConfiguration()
    {
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
