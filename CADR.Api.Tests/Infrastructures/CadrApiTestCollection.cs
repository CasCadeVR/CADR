using Xunit;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Колекция для интеграционных тестов Cadr Апи
/// </summary>
[CollectionDefinition(nameof(CadrApiTestCollection))]
public class CadrApiTestCollection : ICollectionFixture<CadrApiFixture> { }
