using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using CADR.Administrations.Services.AutoMappers;
using Xunit;

namespace CADR.Administrations.Services.Tests;

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
            opts.AddProfile<AdministrationServiceProfile>();
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
