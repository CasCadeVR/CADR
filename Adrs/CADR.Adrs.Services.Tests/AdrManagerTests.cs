using Ahatornn.TestGenerator;
using AutoMapper;
using CADR.Administrations.Entities;
using CADR.Administrations.Entities.Enums;
using CADR.Adrs.Entities;
using CADR.Adrs.Services.Adrs;
using CADR.Adrs.Services.AutoMappers;
using CADR.Adrs.Services.Contracts.Exceptions;
using CADR.Adrs.Services.Contracts.Interfaces;
using CADR.Adrs.Services.Contracts.Models.Adrs;
using CADR.Adrs.Services.Tests.UnitOfWork;
using CADR.Context.Tests;
using FluentAssertions;
using Xunit;
using ContractEnums = CADR.Adrs.Services.Contracts.Models.Enums;
using EntityEnums = CADR.Adrs.Entities.Enums;

namespace CADR.Adrs.Services.Tests;

/// <summary>
/// Тесты на <see cref="IAdrManager"/>
/// </summary>
public class AdrManagerTests : CadrContextInMemory
{
    private readonly IAdrManager adrManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AdrManagerTests"/>
    /// </summary>
    public AdrManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdrServiceProfile());
        });
        var unitOfWorkMock = new TestUnitOfWork(WriterContext, Context, Context);

        adrManager = new AdrManager(unitOfWorkMock.AdrUnitOfWork,
            config.CreateMapper(),
            unitOfWorkMock.UserOrganizationReadRepository,
            unitOfWorkMock.UserReadRepository);
    }

    /// <summary>
    /// Создание ADR работает и назначает первый номер
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = architect.Id;
            x.Status = ContractEnums.AdrStatus.Draft;
        });

        // Act
        var result = await adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                model.Title,
                model.OrganizationId,
                AuthorId = model.AuthorId,
                Status = ContractEnums.AdrStatus.Draft,
                Number = 1,
            });
        Context.Set<Adr>().Should().ContainSingle(x => x.Id == result.Id);
    }

    /// <summary>
    /// Создание ADR назначает следующий номер после существующих, включая удалённые
    /// </summary>
    [Fact]
    public async Task CreateShouldAssignNextNumber()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var existingAdr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.Number = 5;
        });
        var deletedAdr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.Number = 7;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(existingAdr, deletedAdr);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = architect.Id;
            x.Status = ContractEnums.AdrStatus.Draft;
        });

        // Act
        var result = await adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        result.Number.Should().Be(8);
    }

    /// <summary>
    /// Создание ADR выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowDeny()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
        });

        // Act
        Func<Task> act = () => adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Создание ADR создаёт разделы по шаблону
    /// </summary>
    [Fact]
    public async Task CreateShouldInstantiateSectionsFromTemplate()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = null);
        var templateSection1 = TestEntityProvider.Shared.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = template.Id;
            x.Position = 1;
            x.Placeholder = $"Placeholder{Guid.NewGuid()}";
        });
        var templateSection2 = TestEntityProvider.Shared.Create<AdrTemplateSection>(x =>
        {
            x.TemplateId = template.Id;
            x.Position = 2;
            x.Placeholder = $"Placeholder{Guid.NewGuid()}";
        });
        await Context.AddRangeAsync(template, templateSection1, templateSection2);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = architect.Id;
            x.Status = ContractEnums.AdrStatus.Draft;
            x.TemplateId = template.Id;
        });

        // Act
        var result = await adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        result.Sections.Should()
            .HaveCount(2)
            .And.BeEquivalentTo(new[]
            {
                new { Position = 1, templateSection1.Title, templateSection1.Hint, templateSection1.Placeholder, },
                new { Position = 2, templateSection2.Title, templateSection2.Hint, templateSection2.Placeholder, },
            });
    }

    /// <summary>
    /// Создание ADR выдаёт ошибку: шаблон из другой организации
    /// </summary>
    [Fact]
    public async Task CreateShouldThrowTemplateNotAvailable()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var otherOrganization = TestEntityProvider.Shared.Create<Organization>();
        var template = TestEntityProvider.Shared.Create<AdrTemplate>(x => x.OrganizationId = otherOrganization.Id);
        await Context.AddRangeAsync(otherOrganization, template);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = architect.Id;
            x.Status = ContractEnums.AdrStatus.Draft;
            x.TemplateId = template.Id;
        });

        // Act
        Func<Task> act = () => adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Создание ADR со статусом Proposed автоматически утверждает при нулевом пороге
    /// </summary>
    [Fact]
    public async Task CreateShouldAutoApproveWhenThresholdZero()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        await Context.AddAsync(TestEntityProvider.Shared.Create<AdrOrganizationSettings>(x =>
        {
            x.OrganizationId = organization.Id;
            x.LikesRequiredForApproval = 0;
        }));
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<CreateAdrModel>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = architect.Id;
            x.Status = ContractEnums.AdrStatus.Proposed;
        });

        // Act
        var result = await adrManager.CreateAdrAsync(model, CancellationToken.None);

        // Assert
        result.Status.Should().Be(ContractEnums.AdrStatus.Approved);
    }

    /// <summary>
    /// Получение ADR возвращает Score и голос пользователя
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnScoreAndUserVote()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, user.Id, EntityEnums.AdrStatus.Proposed);
        var like = TestEntityProvider.Shared.Create<AdrVote>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = user.Id;
            x.Value = EntityEnums.AdrVoteType.Like;
        });
        var dislike = TestEntityProvider.Shared.Create<AdrVote>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = voter.Id;
            x.Value = EntityEnums.AdrVoteType.Dislike;
        });
        await Context.AddRangeAsync(voter, like, dislike);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrManager.GetByIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                adr.Id,
                adr.Number,
                adr.Title,
                Score = 0,
                UserVote = (ContractEnums.AdrVoteType?)ContractEnums.AdrVoteType.Like,
            });
    }

    /// <summary>
    /// Получение списка ADR организации возвращает только активные
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnOnlyActive()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var adr1 = await SeedAdrAsync(organization, user.Id, EntityEnums.AdrStatus.Draft);
        var adr2 = await SeedAdrAsync(organization, user.Id, EntityEnums.AdrStatus.Draft, asDeleted: true);

        // Act
        var result = await adrManager.GetByOrganizationIdAsync(organization.Id, user.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == adr1.Id);
    }

    /// <summary>
    /// Обновление ADR архитектором работает и заменяет разделы
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var oldSection = TestEntityProvider.Shared.Create<AdrSection>(x =>
        {
            x.AdrId = adr.Id;
            x.Position = 1;
        });
        await Context.AddAsync(oldSection);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<UpdateAdrModel>(x =>
        {
            x.Id = adr.Id;
            x.Number = adr.Number;
            x.Status = ContractEnums.AdrStatus.Draft;
            x.OrganizationId = organization.Id;
            x.AuthorId = adr.AuthorId;
            x.UserId = architect.Id;
            x.Sections = new[]
            {
                new AdrSectionModel { Position = 1, Title = $"T1{Guid.NewGuid()}", Content = $"C1{Guid.NewGuid()}", AdrId = adr.Id, },
                new AdrSectionModel  { Position = 2, Title = $"T2{Guid.NewGuid()}", Content = $"C2{Guid.NewGuid()}", AdrId = adr.Id, },
            };
        });

        // Act
        var result = await adrManager.UpdateAdrAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Title, });
        result.Sections.Should().HaveCount(2);
        Context.Set<AdrSection>().Count(x => x.AdrId == adr.Id).Should().Be(3);
        Context.Set<AdrSection>().First(x => x.Id == oldSection.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrSection>().Count(x => x.AdrId == adr.Id && x.DeletedAt == null).Should().Be(2);
    }

    /// <summary>
    /// Обновление ADR выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task UpdateShouldThrowDeny()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var adr = await SeedAdrAsync(organization, Guid.NewGuid(), EntityEnums.AdrStatus.Draft);
        var model = TestEntityProvider.Shared.Create<UpdateAdrModel>(x =>
        {
            x.Id = adr.Id;
            x.Number = adr.Number;
            x.Status = ContractEnums.AdrStatus.Draft;
            x.OrganizationId = organization.Id;
            x.AuthorId = adr.AuthorId;
            x.UserId = user.Id;
        });

        // Act
        Func<Task> act = () => adrManager.UpdateAdrAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Удаление ADR архитектором работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var section = TestEntityProvider.Shared.Create<AdrSection>(x => x.AdrId = adr.Id);
        await Context.AddAsync(section);
        await UnitOfWork.SaveChangesAsync();
        var model = TestEntityProvider.Shared.Create<DeleteAdrModel>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = architect.Id;
        });

        // Act
        Func<Task> act = () => adrManager.DeleteAdrAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).DeletedAt.Should().NotBeNull();
        Context.Set<AdrSection>().First(x => x.Id == section.Id).DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Смена статуса Draft - Proposed архитектором работает
    /// </summary>
    [Fact]
    public async Task ChangeStatusDraftToProposedShouldWork()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var model = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = architect.Id,
            Status = ContractEnums.AdrStatus.Proposed,
        };

        // Act
        Func<Task> act = () => adrManager.ChangeStatusAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.Proposed);
    }

    /// <summary>
    /// Смена статуса выдаёт ошибку: недопустимый переход
    /// </summary>
    [Fact]
    public async Task ChangeStatusShouldThrowOnInvalidTransition()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var model = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = architect.Id,
            Status = ContractEnums.AdrStatus.Approved,
        };

        // Act
        Func<Task> act = () => adrManager.ChangeStatusAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Смена статуса Proposed - Rejected администратором работает
    /// </summary>
    [Fact]
    public async Task ChangeStatusProposedToRejectedByAdminShouldWork()
    {
        //Arrange
        var (organization, admin) = await SeedOrganizationUserAsync(Role.Admin);
        var adr = await SeedAdrAsync(organization, admin.Id, EntityEnums.AdrStatus.Proposed);
        var rejectModel = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = admin.Id,
            Status = ContractEnums.AdrStatus.Rejected,
        };

        // Act
        Func<Task> rejectAct = () => adrManager.ChangeStatusAsync(rejectModel, CancellationToken.None);

        // Assert
        await rejectAct.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.Rejected);
    }

    /// <summary>
    /// Повторное открытие Rejected - Proposed работает
    /// </summary>
    [Fact]
    public async Task ChangeStatusRejectedToReopenByAdminShouldWork()
    {
        //Arrange
        var (organization, admin) = await SeedOrganizationUserAsync(Role.Admin);
        var adr = await SeedAdrAsync(organization, admin.Id, EntityEnums.AdrStatus.Rejected);

        var reopenModel = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = admin.Id,
            Status = ContractEnums.AdrStatus.Proposed,
        };

        // Act
        Func<Task> reopenAct = () => adrManager.ChangeStatusAsync(reopenModel, CancellationToken.None);

        // Assert
        await reopenAct.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.Proposed);
    }


    /// <summary>
    /// Смена статуса Approved - Deprecated автором работает
    /// </summary>
    [Fact]
    public async Task ChangeStatusApprovedToDeprecatedByAuthorShouldWork()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.User);
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Approved);
        var model = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = author.Id,
            Status = ContractEnums.AdrStatus.Deprecated,
        };

        // Act
        Func<Task> act = () => adrManager.ChangeStatusAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.Deprecated);
    }

    /// <summary>
    /// Смена статуса выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task ChangeStatusShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = Guid.NewGuid();
            x.Status = EntityEnums.AdrStatus.Draft;
        });
        await Context.AddRangeAsync(organization, user, adr);
        await UnitOfWork.SaveChangesAsync();
        var model = new ChangeAdrStatusModel
        {
            AdrId = adr.Id,
            UserId = user.Id,
            Status = ContractEnums.AdrStatus.Proposed,
        };

        // Act
        Func<Task> act = () => adrManager.ChangeStatusAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Голосование архитектором работает
    /// </summary>
    [Fact]
    public async Task VoteShouldWork()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Draft);
        await Context.AddAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = voter.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.Architect;
        }));
        await UnitOfWork.SaveChangesAsync();
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = voter.Id,
            Vote = ContractEnums.AdrVoteType.Like,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrVote>().Should()
            .ContainSingle(x => x.AdrId == adr.Id && x.UserId == voter.Id && x.Value == EntityEnums.AdrVoteType.Like);
    }

    /// <summary>
    /// Повторное голосование меняет существующий голос
    /// </summary>
    [Fact]
    public async Task VoteShouldUpdateExistingVote()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Draft);
        var vote = TestEntityProvider.Shared.Create<AdrVote>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = voter.Id;
            x.Value = EntityEnums.AdrVoteType.Like;
        });
        await Context.AddRangeAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = voter.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.Architect;
        }), vote);
        await UnitOfWork.SaveChangesAsync();
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = voter.Id,
            Vote = ContractEnums.AdrVoteType.Dislike,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrVote>().Should()
            .ContainSingle(x => x.AdrId == adr.Id && x.UserId == voter.Id && x.Value == EntityEnums.AdrVoteType.Dislike);
    }

    /// <summary>
    /// Голосование за собственный ADR выдаёт ошибку
    /// </summary>
    [Fact]
    public async Task VoteOwnAdrShouldThrow()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = architect.Id,
            Vote = ContractEnums.AdrVoteType.Like,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrInvalidOperationException>();
    }

    /// <summary>
    /// Голосование выдаёт ошибку: не хватает прав
    /// </summary>
    [Fact]
    public async Task VoteShouldThrowDeny()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var user = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Draft);
        await Context.AddAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = user.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.User;
        }));
        await UnitOfWork.SaveChangesAsync();
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = user.Id,
            Vote = ContractEnums.AdrVoteType.Like,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Голосование Proposed ADR автоматически утверждает при достижении порога
    /// </summary>
    [Fact]
    public async Task VoteShouldAutoApproveProposed()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Proposed);
        await Context.AddAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = voter.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.Architect;
        }));
        await UnitOfWork.SaveChangesAsync();
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = voter.Id,
            Vote = ContractEnums.AdrVoteType.Like,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.Approved);
    }

    /// <summary>
    /// Голосование против Approved ADR переводит его в NeedsRevision
    /// </summary>
    [Fact]
    public async Task VoteShouldMoveApprovedToNeedsRevision()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Approved);
        await Context.AddAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = voter.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.Architect;
        }));
        await UnitOfWork.SaveChangesAsync();
        var model = new VoteAdrModel
        {
            AdrId = adr.Id,
            UserId = voter.Id,
            Vote = ContractEnums.AdrVoteType.Dislike,
        };

        // Act
        Func<Task> act = () => adrManager.VoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<Adr>().First(x => x.Id == adr.Id).Status.Should().Be(EntityEnums.AdrStatus.NeedsRevision);
    }

    /// <summary>
    /// Снятие голоса работает
    /// </summary>
    [Fact]
    public async Task WithdrawVoteShouldWork()
    {
        //Arrange
        var (organization, author) = await SeedOrganizationUserAsync(Role.Architect);
        var voter = TestEntityProvider.Shared.Create<User>();
        var adr = await SeedAdrAsync(organization, author.Id, EntityEnums.AdrStatus.Proposed);
        var vote = TestEntityProvider.Shared.Create<AdrVote>(x =>
        {
            x.AdrId = adr.Id;
            x.UserId = voter.Id;
            x.Value = EntityEnums.AdrVoteType.Like;
        });
        await Context.AddRangeAsync(TestEntityProvider.Shared.Create<UserOrganization>(x =>
        {
            x.UserId = voter.Id;
            x.OrganizationId = organization.Id;
            x.Role = Role.Architect;
        }), vote);
        await UnitOfWork.SaveChangesAsync();
        var model = new WithdrawVoteAdrModel
        {
            AdrId = adr.Id,
            UserId = voter.Id,
        };

        // Act
        Func<Task> act = () => adrManager.WithdrawVoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        Context.Set<AdrVote>().First(x => x.Id == vote.Id).DeletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Снятие голоса выдаёт ошибку: голос не найден
    /// </summary>
    [Fact]
    public async Task WithdrawVoteShouldThrowNotFound()
    {
        //Arrange
        var (organization, architect) = await SeedOrganizationUserAsync(Role.Architect);
        var adr = await SeedAdrAsync(organization, architect.Id, EntityEnums.AdrStatus.Draft);
        var model = new WithdrawVoteAdrModel
        {
            AdrId = adr.Id,
            UserId = architect.Id,
        };

        // Act
        Func<Task> act = () => adrManager.WithdrawVoteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrEntityNotFoundException<AdrVote>>();
    }

    /// <summary>
    /// Получение списка последних ADR возвращает самые свежие активные с учётом лимита
    /// </summary>
    [Fact]
    public async Task GetRecentShouldWork()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var now = DateTimeOffset.UtcNow;
        var oldest = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.UpdatedAt = now.AddDays(-3);
        });
        var middle = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.UpdatedAt = now.AddDays(-2);
        });
        var newest = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Proposed;
            x.UpdatedAt = now.AddDays(-1);
        });
        var deleted = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.UpdatedAt = now;
            x.DeletedAt = now;
        });
        await Context.AddRangeAsync(oldest, middle, newest, deleted);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrManager.GetRecentAsync(organization.Id, user.Id, 2, CancellationToken.None);

        // Assert
        result.Select(x => x.Id).Should()
            .BeEquivalentTo(new[] { newest.Id, middle.Id }, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Получение списка последних ADR выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task GetRecentShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        await Context.AddAsync(organization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrManager.GetRecentAsync(organization.Id, Guid.NewGuid(), 10, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение списка ADR по статусам возвращает только подходящие активные ADR
    /// </summary>
    [Fact]
    public async Task GetByStatusesShouldWork()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var draft = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.Number = 1;
        });
        var proposed = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Proposed;
            x.Number = 2;
        });
        var approved = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Approved;
            x.Number = 3;
        });
        var needsRevision = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.NeedsRevision;
            x.Number = 4;
        });
        var deletedProposed = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Proposed;
            x.Number = 5;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        await Context.AddRangeAsync(draft, proposed, approved, needsRevision, deletedProposed);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrManager.GetByStatusesAsync(organization.Id,
            new[] { ContractEnums.AdrStatus.Proposed, ContractEnums.AdrStatus.NeedsRevision },
            user.Id,
            CancellationToken.None);

        // Assert
        result.Select(x => x.Id).Should()
            .BeEquivalentTo(new[] { proposed.Id, needsRevision.Id }, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Получение списка ADR по статусам выдаёт ошибку: пользователь не состоит в организации
    /// </summary>
    [Fact]
    public async Task GetByStatusesShouldThrowNotMember()
    {
        //Arrange
        var organization = TestEntityProvider.Shared.Create<Organization>();
        await Context.AddAsync(organization);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => adrManager.GetByStatusesAsync(organization.Id,
            new[] { ContractEnums.AdrStatus.Proposed },
            Guid.NewGuid(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdrAccessException>();
    }

    /// <summary>
    /// Получение ADR возвращает имя и логин автора
    /// </summary>
    [Fact]
    public async Task GetByIdShouldFillAuthor()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var adr = await SeedAdrAsync(organization, user.Id, EntityEnums.AdrStatus.Draft);

        // Act
        var result = await adrManager.GetByIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.AuthorName.Should().Be(user.Name);
        result.AuthorLogin.Should().Be(user.Login);
    }

    /// <summary>
    /// Получение ADR возвращает путь по вложенным папкам от корня к папке ADR
    /// </summary>
    [Fact]
    public async Task GetByIdShouldFillFolderPath()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var rootFolder = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
        });
        var childFolder = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.ParentAdrFolderId = rootFolder.Id;
        });
        var adr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.ParentAdrFolderId = childFolder.Id;
        });
        await Context.AddRangeAsync(rootFolder, childFolder, adr);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrManager.GetByIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.FolderPath.Should().NotBeNull();
        result.FolderPath!.Select(x => x.Id).Should()
            .BeEquivalentTo(new[] { rootFolder.Id, childFolder.Id }, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Получение ADR возвращает пустой путь, если ADR лежит в корне организации
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnEmptyFolderPathForRootAdr()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var adr = await SeedAdrAsync(organization, user.Id, EntityEnums.AdrStatus.Draft);

        // Act
        var result = await adrManager.GetByIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.FolderPath.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Получение ADR возвращает пустой путь без ошибки, если папка ADR была удалена
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnEmptyFolderPathForDeletedFolder()
    {
        //Arrange
        var (organization, user) = await SeedOrganizationUserAsync(Role.User);
        var deletedFolder = TestEntityProvider.Shared.Create<AdrFolder>(x =>
        {
            x.OrganizationId = organization.Id;
            x.DeletedAt = DateTimeOffset.UtcNow;
        });
        var adr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = user.Id;
            x.Status = EntityEnums.AdrStatus.Draft;
            x.ParentAdrFolderId = deletedFolder.Id;
        });
        await Context.AddRangeAsync(deletedFolder, adr);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await adrManager.GetByIdAsync(adr.Id, user.Id, CancellationToken.None);

        // Assert
        result.FolderPath.Should().NotBeNull().And.BeEmpty();
    }

    private async Task<(Organization organization, User user)> SeedOrganizationUserAsync(Role role)
    {
        var organization = TestEntityProvider.Shared.Create<Organization>();
        var user = TestEntityProvider.Shared.Create<User>();
        await Context.AddRangeAsync(organization,
            user,
            TestEntityProvider.Shared.Create<UserOrganization>(x =>
            {
                x.UserId = user.Id;
                x.OrganizationId = organization.Id;
                x.Role = role;
            }));
        await UnitOfWork.SaveChangesAsync();
        return (organization, user);
    }

    private async Task<Adr> SeedAdrAsync(Organization organization, Guid authorId, EntityEnums.AdrStatus status, bool? asDeleted = false)
    {
        var adr = TestEntityProvider.Shared.Create<Adr>(x =>
        {
            x.OrganizationId = organization.Id;
            x.AuthorId = authorId;
            x.Status = status;
        });
        if (asDeleted == true)
        {
            adr.DeletedAt = DateTime.UtcNow;
        }
        await Context.AddAsync(adr);
        await UnitOfWork.SaveChangesAsync();
        return adr;
    }
}
