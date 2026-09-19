using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CADR.Adrs.Entities.Configurations.Seeding;

/// <summary>Конфигурация начальных данных для <see cref="AdrTemplateSection"/></summary>
public class AdrTemplateSectionSeeder : IEntityTypeConfiguration<AdrTemplateSection>
{
    private readonly AdrTemplateSection contextSection = new()
    {
        Title = "Контекст",
        Hint = "Что заставило меня принять такое решение?",
        Placeholder = "Опишите контекст и суть проблемы — например, в свободной форме (двумя-тремя предложениями) или в виде наглядной истории." +
                        "Можно сформулировать проблему как вопрос. Рекомендуется добавить ссылки на доски для совместной работы или системы управления задачами." +
                        "Четко обозначьте область действия принимаемого решения, например, выделив или указав конкретные элементы архитектуры (компоненты, коннекторы и т. д.).",
        Position = 1,
    };

    private readonly AdrTemplateSection solutionSection = new()
    {
        Title = "Решение",
        Hint = "Решение и соответствующее обоснование",
        Placeholder = "{название варианта 1}\r\n{название варианта 2}\r\n{название варианта 3}\r\n…",
        Position = 2,
    };

    private readonly AdrTemplateSection consequencesSection = new()
    {
        Title = "Последствия",
        Hint = "На что повлияет данное решение?",
        Placeholder = "Выбранный вариант: «{название варианта 1}», поскольку {обоснование, например: единственный вариант, отвечающий решающему критерию | вариант, устраняющий воздействие {воздействие} | … | вариант, показавший наилучший результат}." +
                    "\r\nПоследствия\r\nХорошие, потому что {положительные последствия, например, улучшение одного или нескольких желаемых качеств, …}" +
                    "\r\nПлохие, потому что {отрицательные последствия, например, ухудшение одного или нескольких желаемых качеств, …}\r\n…",
        Position = 3,
    };

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AdrTemplateSection> builder)
        => builder.HasData(
        [
            // Минимальный

            new AdrTemplateSection()
            {
                Id = Guid.Parse("a6503c25-bb02-4b3b-82b4-abe7576c2581"),
                Title = contextSection.Title,
                Hint = contextSection.Hint,
                Placeholder = contextSection.Placeholder,
                Position = contextSection.Position,
                TemplateId = Guid.Parse("fe644bbb-416f-4af0-abc0-5e0144db26c0"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("0f1f1d4a-4898-4ee0-8ff2-ae34ab95ea6c"),
                Title = solutionSection.Title,
                Hint = solutionSection.Hint,
                Placeholder = solutionSection.Placeholder,
                Position = solutionSection.Position,
                TemplateId = Guid.Parse("fe644bbb-416f-4af0-abc0-5e0144db26c0"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("c9cbb06f-d455-4ef9-8435-8832fcbb66fd"),
                Title = consequencesSection.Title,
                Hint = consequencesSection.Hint,
                Placeholder = consequencesSection.Placeholder,
                Position = consequencesSection.Position,
                TemplateId = Guid.Parse("fe644bbb-416f-4af0-abc0-5e0144db26c0"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            // Базовый

            new AdrTemplateSection()
            {
                Id = Guid.Parse("68d5fdba-fe6d-451c-bd18-bc64b3a180cf"),
                Title = contextSection.Title,
                Hint = contextSection.Hint,
                Placeholder = contextSection.Placeholder,
                Position = contextSection.Position,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("4b1e7519-e638-4d05-bec8-78342634e730"),
                Title = solutionSection.Title,
                Hint = solutionSection.Hint,
                Placeholder = solutionSection.Placeholder,
                Position = solutionSection.Position,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("ee962af1-c86b-45fe-ab22-4a8110a160f5"),
                Title = consequencesSection.Title,
                Hint = consequencesSection.Hint,
                Placeholder = consequencesSection.Placeholder,
                Position = consequencesSection.Position,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("d5b7e4b1-7032-4ce5-993a-2e8da86c5709"),
                Title = "Соблюдение требований",
                Hint = "Как я обеспечу соблюдение этого решения?",
                Placeholder = "Чтобы убедиться, что решение «{название решения}» соблюдается на практике, мы будем" +
                    "\r\n{способ проверки, например: периодически проверять {что проверяем: процесс | документы | результат работы | …}" +
                    "\r\nв формате {вид проверки: аудит | контрольная точка | ревью артефактов | опрос участников | …}" +
                    "\r\n| отслеживать показатель «{метрика}» и сверять его с целевым значением {значение}" +
                    "\r\n| поручить {роль ответственного} проверять соответствие с периодичностью {периодичность: раз в месяц | каждую итерацию | ежеквартально | …}" +
                    "\r\n| …}.\r\n\r\nОтклонения от решения фиксируются {где фиксируются: в комментариях к этому ADR | в системе задач | в журнале проверок | …}," +
                    "\r\nпосле чего {что делаем дальше: проводится разбор с {роль, принимающая решение} | решение уточняется или отменяется через новое ADR | …}.",
                Position = 4,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("500bf9b4-349b-490e-a475-49ba1da3bdc9"),
                Title = "Плюсы и минусы вариантов",
                Hint = "Перечислите все рассмотренные варианты",
                Placeholder = "{название варианта 1}\r\n{пример | описание | ссылка на дополнительную информацию | …}" +
                    "\r\n\r\nХорошо, потому что {аргумент а}\r\nХорошо, потому что {аргумент б}\r\nНейтрально, потому что {аргумент в}" +
                    "\r\nПлохо, потому что {аргумент г}\r\n…\r\n\r\n{название другого варианта}\r\n{пример | описание | ссылка на дополнительную информацию | …}" +
                    "\r\n\r\nХорошо, потому что {аргумент а}\r\nНейтрально, потому что {аргумент б}\r\nПлохо, потому что {аргумент в}\r\n…",
                Position = 5,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },

            new AdrTemplateSection()
            {
                Id = Guid.Parse("ec56b0ef-3a7d-44b0-a2ca-0b74e2f1023e"),
                Title = "Дополнительная информация",
                Hint = "Укажите всё, что усиливает доверие к решению и помогает в дальнейшем",
                Placeholder = "{дополнительные доказательства или обоснование уверенности в принятом решении, например: результаты расчётов | прототип | обратная связь {от кого} | …}" +
                    "\r\n\r\n{договорённости команды, например: согласовано с {роль/участники} {дата} | решение принято консенсусом | …}" +
                    "\r\n\r\n{когда и как решение должно быть реализовано, например: внедряется начиная с {этап/дата} | ответственность за реализацию — {роль} | …}" +
                    "\r\n\r\n{условия пересмотра решения, например: пересматриваем, если {условие} | revisit {дата/событие} | …}" +
                    "\r\n\r\n{ссылки на другие решения и ресурсы, например: связанное ADR-{номер} | документация | исследование | …}",
                Position = 6,
                TemplateId = Guid.Parse("94b3e49d-e314-4b0e-b620-35e38759feaa"),
                CreatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                UpdatedAt = DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
                CreatedBy = "system",
                UpdatedBy = "system",
            },
        ]);
}
