using Xunit;

namespace CADR.Api.Tests.Infrastructures;

/// <summary>
/// Колекция для интеграционных тестов Cadr Апи с авторизованным пользователем
/// </summary>
[CollectionDefinition(nameof(CadrApiAuthorizedUserTestCollection))]
public class CadrApiAuthorizedUserTestCollection : ICollectionFixture<CadrApiAuthorizedUserFixture>;
