using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CADR.Context.Migrations
{
    /// <inheritdoc />
    public partial class adrsModuleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdrFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentAdrFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrFolders_AdrFolders_ParentAdrFolderId",
                        column: x => x.ParentAdrFolderId,
                        principalTable: "AdrFolders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdrFolders_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdrOrganizationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LikesRequiredForApproval = table.Column<int>(type: "integer", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrOrganizationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrOrganizationSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrTemplates_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Adrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentAdrFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    AdrFolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adrs_AdrFolders_AdrFolderId",
                        column: x => x.AdrFolderId,
                        principalTable: "AdrFolders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adrs_AdrFolders_ParentAdrFolderId",
                        column: x => x.ParentAdrFolderId,
                        principalTable: "AdrFolders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adrs_AdrTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "AdrTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adrs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Adrs_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrTemplateSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Hint = table.Column<string>(type: "text", nullable: false),
                    Placeholder = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrTemplateSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrTemplateSections_AdrTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "AdrTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    AdrId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrComments_Adrs_AdrId",
                        column: x => x.AdrId,
                        principalTable: "Adrs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdrComments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SourceAdrId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetAdrId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrLinks_Adrs_SourceAdrId",
                        column: x => x.SourceAdrId,
                        principalTable: "Adrs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdrLinks_Adrs_TargetAdrId",
                        column: x => x.TargetAdrId,
                        principalTable: "Adrs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AdrId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrSections_Adrs_AdrId",
                        column: x => x.AdrId,
                        principalTable: "Adrs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdrVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    AdrId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdrVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdrVotes_Adrs_AdrId",
                        column: x => x.AdrId,
                        principalTable: "Adrs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdrVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AdrTemplates",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Name", "OrganizationId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Базовый", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("fe644bbb-416f-4af0-abc0-5e0144db26c0"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Минимальный", null, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" }
                });

            migrationBuilder.InsertData(
                table: "AdrTemplateSections",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "Hint", "Placeholder", "Position", "TemplateId", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0f1f1d4a-4898-4ee0-8ff2-ae34ab95ea6c"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Решение и соответствующее обоснование", "{название варианта 1}\r\n{название варианта 2}\r\n{название варианта 3}\r\n…", 2, new Guid("fe644bbb-416f-4af0-abc0-5e0144db26c0"), "Решение", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("4b1e7519-e638-4d05-bec8-78342634e730"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Решение и соответствующее обоснование", "{название варианта 1}\r\n{название варианта 2}\r\n{название варианта 3}\r\n…", 2, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Решение", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("500bf9b4-349b-490e-a475-49ba1da3bdc9"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Перечислите все рассмотренные варианты", "{название варианта 1}\r\n{пример | описание | ссылка на дополнительную информацию | …}\r\n\r\nХорошо, потому что {аргумент а}\r\nХорошо, потому что {аргумент б}\r\nНейтрально, потому что {аргумент в}\r\nПлохо, потому что {аргумент г}\r\n…\r\n\r\n{название другого варианта}\r\n{пример | описание | ссылка на дополнительную информацию | …}\r\n\r\nХорошо, потому что {аргумент а}\r\nНейтрально, потому что {аргумент б}\r\nПлохо, потому что {аргумент в}\r\n…", 5, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Плюсы и минусы вариантов", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("68d5fdba-fe6d-451c-bd18-bc64b3a180cf"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Что заставило меня принять такое решение?", "Опишите контекст и суть проблемы — например, в свободной форме (двумя-тремя предложениями) или в виде наглядной истории.Можно сформулировать проблему как вопрос. Рекомендуется добавить ссылки на доски для совместной работы или системы управления задачами.Четко обозначьте область действия принимаемого решения, например, выделив или указав конкретные элементы архитектуры (компоненты, коннекторы и т. д.).", 1, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Контекст", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("a6503c25-bb02-4b3b-82b4-abe7576c2581"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Что заставило меня принять такое решение?", "Опишите контекст и суть проблемы — например, в свободной форме (двумя-тремя предложениями) или в виде наглядной истории.Можно сформулировать проблему как вопрос. Рекомендуется добавить ссылки на доски для совместной работы или системы управления задачами.Четко обозначьте область действия принимаемого решения, например, выделив или указав конкретные элементы архитектуры (компоненты, коннекторы и т. д.).", 1, new Guid("fe644bbb-416f-4af0-abc0-5e0144db26c0"), "Контекст", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("c9cbb06f-d455-4ef9-8435-8832fcbb66fd"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "На что повлияет данное решение?", "Выбранный вариант: «{название варианта 1}», поскольку {обоснование, например: единственный вариант, отвечающий решающему критерию | вариант, устраняющий воздействие {воздействие} | … | вариант, показавший наилучший результат}.\r\nПоследствия\r\nХорошие, потому что {положительные последствия, например, улучшение одного или нескольких желаемых качеств, …}\r\nПлохие, потому что {отрицательные последствия, например, ухудшение одного или нескольких желаемых качеств, …}\r\n…", 3, new Guid("fe644bbb-416f-4af0-abc0-5e0144db26c0"), "Последствия", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("d5b7e4b1-7032-4ce5-993a-2e8da86c5709"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Как я обеспечу соблюдение этого решения?", "Чтобы убедиться, что решение «{название решения}» соблюдается на практике, мы будем\r\n{способ проверки, например: периодически проверять {что проверяем: процесс | документы | результат работы | …}\r\nв формате {вид проверки: аудит | контрольная точка | ревью артефактов | опрос участников | …}\r\n| отслеживать показатель «{метрика}» и сверять его с целевым значением {значение}\r\n| поручить {роль ответственного} проверять соответствие с периодичностью {периодичность: раз в месяц | каждую итерацию | ежеквартально | …}\r\n| …}.\r\n\r\nОтклонения от решения фиксируются {где фиксируются: в комментариях к этому ADR | в системе задач | в журнале проверок | …},\r\nпосле чего {что делаем дальше: проводится разбор с {роль, принимающая решение} | решение уточняется или отменяется через новое ADR | …}.", 4, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Соблюдение требований", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("ec56b0ef-3a7d-44b0-a2ca-0b74e2f1023e"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "Укажите всё, что усиливает доверие к решению и помогает в дальнейшем", "{дополнительные доказательства или обоснование уверенности в принятом решении, например: результаты расчётов | прототип | обратная связь {от кого} | …}\r\n\r\n{договорённости команды, например: согласовано с {роль/участники} {дата} | решение принято консенсусом | …}\r\n\r\n{когда и как решение должно быть реализовано, например: внедряется начиная с {этап/дата} | ответственность за реализацию — {роль} | …}\r\n\r\n{условия пересмотра решения, например: пересматриваем, если {условие} | revisit {дата/событие} | …}\r\n\r\n{ссылки на другие решения и ресурсы, например: связанное ADR-{номер} | документация | исследование | …}", 6, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Дополнительная информация", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" },
                    { new Guid("ee962af1-c86b-45fe-ab22-4a8110a160f5"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system", null, "На что повлияет данное решение?", "Выбранный вариант: «{название варианта 1}», поскольку {обоснование, например: единственный вариант, отвечающий решающему критерию | вариант, устраняющий воздействие {воздействие} | … | вариант, показавший наилучший результат}.\r\nПоследствия\r\nХорошие, потому что {положительные последствия, например, улучшение одного или нескольких желаемых качеств, …}\r\nПлохие, потому что {отрицательные последствия, например, ухудшение одного или нескольких желаемых качеств, …}\r\n…", 3, new Guid("94b3e49d-e314-4b0e-b620-35e38759feaa"), "Последствия", new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "system" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdrComment_CreatedAt",
                table: "AdrComments",
                columns: new[] { "AdrId", "CreatedAt" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrComments_AuthorId",
                table: "AdrComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AdrFolder_Name",
                table: "AdrFolders",
                columns: new[] { "OrganizationId", "ParentAdrFolderId", "Name" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrFolders_ParentAdrFolderId",
                table: "AdrFolders",
                column: "ParentAdrFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_AdrLink_Type",
                table: "AdrLinks",
                columns: new[] { "SourceAdrId", "TargetAdrId", "Type" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrLinks_TargetAdrId",
                table: "AdrLinks",
                column: "TargetAdrId");

            migrationBuilder.CreateIndex(
                name: "IX_AdrOrganizationSettings_OrganizationId",
                table: "AdrOrganizationSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Adr_Number",
                table: "Adrs",
                columns: new[] { "OrganizationId", "Number" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Adr_ParentAdrFolder",
                table: "Adrs",
                columns: new[] { "OrganizationId", "ParentAdrFolderId" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Adr_Status",
                table: "Adrs",
                columns: new[] { "OrganizationId", "Status" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Adrs_AdrFolderId",
                table: "Adrs",
                column: "AdrFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Adrs_AuthorId",
                table: "Adrs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Adrs_ParentAdrFolderId",
                table: "Adrs",
                column: "ParentAdrFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Adrs_TemplateId",
                table: "Adrs",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AdrSection_Position",
                table: "AdrSections",
                columns: new[] { "AdrId", "Position" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrTemplate_Name",
                table: "AdrTemplates",
                columns: new[] { "OrganizationId", "Name" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL AND \"OrganizationId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrTemplateSection_Position",
                table: "AdrTemplateSections",
                columns: new[] { "TemplateId", "Position" },
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrVote_User",
                table: "AdrVotes",
                columns: new[] { "AdrId", "UserId" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdrVotes_UserId",
                table: "AdrVotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdrComments");

            migrationBuilder.DropTable(
                name: "AdrLinks");

            migrationBuilder.DropTable(
                name: "AdrOrganizationSettings");

            migrationBuilder.DropTable(
                name: "AdrSections");

            migrationBuilder.DropTable(
                name: "AdrTemplateSections");

            migrationBuilder.DropTable(
                name: "AdrVotes");

            migrationBuilder.DropTable(
                name: "Adrs");

            migrationBuilder.DropTable(
                name: "AdrFolders");

            migrationBuilder.DropTable(
                name: "AdrTemplates");
        }
    }
}
