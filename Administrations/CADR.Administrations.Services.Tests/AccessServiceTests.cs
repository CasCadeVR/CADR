using Ahatornn.TestGenerator;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Administrations.Repositories;
using CADR.Administrations.Services.Contracts.Exceptions;
using CADR.Administrations.Services.Contracts.Interfaces;
using CADR.Common.Core.Contracts;
using CADR.Context.Tests;
using FluentAssertions;
using Moq;
using Xunit;

namespace CADR.Administrations.Services.Tests
{
    /// <summary>
    /// Тесты для <see cref="AccessService"/>
    /// </summary>
    public class AccessServiceTests : CadrContextInMemory
    {
        private readonly IAccessService accessService;
        private readonly Guid adminId = Guid.NewGuid();

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="AccessServiceTests"/>
        /// </summary>
        public AccessServiceTests()
        {
            var identityProviderMock = new Mock<IIdentityProvider>();
            identityProviderMock.Setup(x => x.Id).Returns(adminId);
            accessService = new AccessService(identityProviderMock.Object, new OrganizationReadRepository(Context));
        }

        /// <summary>
        /// Пользователь является администратором для организации другого пользователя
        /// </summary>
        [Fact]
        public async Task IsAdminForAnyUserOrganizationShouldNotThrowException()
        {
            // Arrange
            var otherUserId = Guid.NewGuid();
            var targetOrganization = TestEntityProvider.Shared.Create<Entities.Organization>();
            await Context.AddRangeAsync(targetOrganization,
                TestEntityProvider.Shared.Create<UserOrganization>(x =>
                {
                    x.UserId = adminId;
                    x.OrganizationId = targetOrganization.Id;
                    x.Role = Role.Admin;
                }),
                TestEntityProvider.Shared.Create<UserOrganization>(x =>
                {
                    x.UserId = otherUserId;
                    x.OrganizationId = targetOrganization.Id;
                }));
            await Context.SaveChangesAsync();

            // Act
            Func<Task> act = () => accessService.IsAdminForAnyUserOrganizationAsync(adminId, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
        }

        /// <summary>
        /// Пользователь не является администратором для организации другого пользователя
        /// </summary>
        [Fact]
        public async Task IsAdminForAnyUserOrganizationShouldThrowException()
        {
            // Arrange
            var otherUserId = Guid.NewGuid();
            var targetOrganization = TestEntityProvider.Shared.Create<Entities.Organization>();
            await Context.AddRangeAsync(targetOrganization,
                TestEntityProvider.Shared.Create<UserOrganization>(x =>
                {
                    x.UserId = adminId;
                    x.OrganizationId = targetOrganization.Id;
                    x.Role = Role.User;
                }),
                TestEntityProvider.Shared.Create<UserOrganization>(x =>
                {
                    x.UserId = otherUserId;
                    x.OrganizationId = targetOrganization.Id;
                }));
            await Context.SaveChangesAsync();

            // Act
            Func<Task> act = () => accessService.IsAdminForAnyUserOrganizationAsync(adminId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<AdministrationAccessException>();
        }
    }
}
