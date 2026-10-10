using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CADR.Context.Migrations
{
    /// <inheritdoc />
    public partial class dbObjectsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ---------- VIEW: adr_summary ----------
            migrationBuilder.Sql("""
                CREATE VIEW adr_summary AS
                SELECT
                    a."Id",
                    a."Number",
                    a."Title",
                    a."Status",
                    a."OrganizationId",
                    a."AuthorId",
                    u."Name"  AS author_name,
                    u."Login" AS author_login,
                    COALESCE(v.score, 0) AS score,
                    COALESCE(s."LikesRequiredForApproval", 1) AS likes_required_for_approval,
                    a."ParentAdrFolderId" AS folder_id,
                    a."CreatedAt",
                    a."UpdatedAt",
                    a."DeletedAt"
                FROM "Adrs" a
                JOIN "Users" u ON u."Id" = a."AuthorId"
                LEFT JOIN (
                    SELECT "AdrId", SUM(CASE WHEN "Value" = 0 THEN 1 ELSE -1 END) AS score
                    FROM "AdrVotes"
                    WHERE "DeletedAt" IS NULL
                    GROUP BY "AdrId"
                ) v ON v."AdrId" = a."Id"
                LEFT JOIN "AdrOrganizationSettings" s
                    ON s."OrganizationId" = a."OrganizationId" AND s."DeletedAt" IS NULL;
                """);

            // ---------- VIEW: organization_members ----------
            migrationBuilder.Sql("""
                CREATE VIEW organization_members AS
                SELECT
                    uo."UserId"      AS user_id,
                    uo."OrganizationId" AS organization_id,
                    uo."Role"        AS role,
                    u."Name"         AS user_name,
                    u."Login"        AS user_login,
                    u."Email"        AS user_email,
                    u."Blocked"      AS user_blocked
                FROM "UserOrganizations" uo
                JOIN "Users" u ON u."Id" = uo."UserId"
                WHERE uo."DeletedAt" IS NULL AND u."DeletedAt" IS NULL AND uo."OrganizationId" IS NOT NULL;
                """);

            // ---------- FUNCTION: create_adr ----------
            migrationBuilder.Sql("""
                CREATE FUNCTION create_adr(
                    p_organization_id uuid,
                    p_author_id uuid,
                    p_folder_id uuid,
                    p_title text)
                RETURNS TABLE (id uuid, number integer)
                LANGUAGE plpgsql AS $$
                DECLARE
                    v_number int;
                    v_threshold int;
                    v_status int;
                    v_id uuid;
                    v_actor varchar(200);
                BEGIN
                    SELECT "Login" INTO v_actor FROM "Users" WHERE "Id" = p_author_id;
                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'Author not found: %', p_author_id;
                    END IF;

                    SELECT COALESCE(MAX("Number"), 0) + 1 INTO v_number
                    FROM "Adrs" WHERE "OrganizationId" = p_organization_id;

                    SELECT COALESCE("LikesRequiredForApproval", 1) INTO v_threshold
                    FROM "AdrOrganizationSettings"
                    WHERE "OrganizationId" = p_organization_id AND "DeletedAt" IS NULL;

                    v_status := CASE WHEN v_threshold = 0 THEN 2 ELSE 0 END;

                    INSERT INTO "Adrs" ("Id", "Number", "Title", "Status", "OrganizationId", "AuthorId",
                                        "ParentAdrFolderId", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy")
                    VALUES (gen_random_uuid(), v_number, p_title, v_status, p_organization_id,
                            p_author_id, p_folder_id, now(), v_actor, now(), v_actor)
                    RETURNING "Id" INTO v_id;

                    RETURN QUERY SELECT v_id, v_number;
                END; $$;
                """);

            // ---------- FUNCTION: vote_adr ----------
            migrationBuilder.Sql("""
                CREATE FUNCTION vote_adr(p_adr_id uuid, p_user_id uuid, p_value int)
                RETURNS void
                LANGUAGE plpgsql AS $$
                DECLARE
                    v_status int;
                    v_author_id uuid;
                BEGIN
                    SELECT "Status", "AuthorId" INTO v_status, v_author_id
                    FROM "Adrs" WHERE "Id" = p_adr_id AND "DeletedAt" IS NULL;

                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'ADR not found: %', p_adr_id;
                    END IF;
                    IF v_author_id = p_user_id THEN
                        RAISE EXCEPTION 'Self-vote is forbidden';
                    END IF;
                    IF v_status NOT IN (1, 2, 4) THEN
                        RAISE EXCEPTION 'Voting is not allowed for ADR status %', v_status;
                    END IF;

                    UPDATE "AdrVotes"
                    SET "Value" = p_value, "DeletedAt" = NULL, "UpdatedAt" = now(),
                        "UpdatedBy" = (SELECT "Login" FROM "Users" WHERE "Id" = p_user_id)
                    WHERE "AdrId" = p_adr_id AND "UserId" = p_user_id;

                    IF NOT FOUND THEN
                        INSERT INTO "AdrVotes" ("Id", "Value", "AdrId", "UserId",
                                                "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy")
                        VALUES (gen_random_uuid(), p_value, p_adr_id, p_user_id,
                                now(), (SELECT "Login" FROM "Users" WHERE "Id" = p_user_id),
                                now(), (SELECT "Login" FROM "Users" WHERE "Id" = p_user_id));
                    END IF;
                END; $$;
                """);

            // ---------- FUNCTION: withdraw_vote_adr ----------
            migrationBuilder.Sql("""
                CREATE FUNCTION withdraw_vote_adr(p_adr_id uuid, p_user_id uuid)
                RETURNS void
                LANGUAGE plpgsql AS $$
                BEGIN
                    UPDATE "AdrVotes"
                    SET "DeletedAt" = now(),
                        "UpdatedAt" = now(),
                        "UpdatedBy" = (SELECT "Login" FROM "Users" WHERE "Id" = p_user_id)
                    WHERE "AdrId" = p_adr_id AND "UserId" = p_user_id AND "DeletedAt" IS NULL;

                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'Vote not found';
                    END IF;
                END; $$;
                """);

            // ---------- FUNCTION: change_adr_status ----------
            migrationBuilder.Sql("""
                CREATE FUNCTION change_adr_status(p_adr_id uuid, p_actor_role int, p_new_status int)
                RETURNS void
                LANGUAGE plpgsql AS $$
                DECLARE
                    v_current int;
                BEGIN
                    SELECT "Status" INTO v_current FROM "Adrs"
                    WHERE "Id" = p_adr_id AND "DeletedAt" IS NULL;
                    IF NOT FOUND THEN
                        RAISE EXCEPTION 'ADR not found';
                    END IF;

                    IF NOT (
                        (v_current = 0 AND p_new_status = 1) OR
                        (v_current = 1 AND p_new_status IN (2, 3) AND p_actor_role >= 2) OR
                        (v_current = 2 AND p_new_status IN (4, 5)) OR
                        (v_current = 3 AND p_new_status = 1 AND p_actor_role >= 2) OR
                        (v_current = 4 AND p_new_status IN (1, 5))
                    ) THEN
                        RAISE EXCEPTION 'Transition % -> % not allowed for role %', v_current, p_new_status, p_actor_role;
                    END IF;

                    UPDATE "Adrs"
                    SET "Status" = p_new_status, "UpdatedAt" = now(),
                        "UpdatedBy" = COALESCE((SELECT "Login" FROM "Users"
                                                WHERE "Id" = (SELECT "AuthorId" FROM "Adrs" WHERE "Id" = p_adr_id)), 'system')
                    WHERE "Id" = p_adr_id;
                END; $$;
                """);

            // ---------- TRIGGER: auto-approval ----------
            migrationBuilder.Sql("""
                CREATE FUNCTION fn_adr_votes_recalc() RETURNS trigger AS $$
                DECLARE
                    v_adr_id uuid;
                    v_score int;
                    v_status int;
                    v_threshold int;
                BEGIN
                    v_adr_id := COALESCE(NEW."AdrId", OLD."AdrId");

                    SELECT "Status" INTO v_status FROM "Adrs" WHERE "Id" = v_adr_id;
                    IF NOT FOUND THEN
                        RETURN NULL;
                    END IF;

                    SELECT COALESCE(SUM(CASE WHEN "Value" = 0 THEN 1 ELSE -1 END), 0) INTO v_score
                    FROM "AdrVotes" WHERE "AdrId" = v_adr_id AND "DeletedAt" IS NULL;

                    SELECT COALESCE(s."LikesRequiredForApproval", 1) INTO v_threshold
                    FROM "Adrs" a
                    LEFT JOIN "AdrOrganizationSettings" s
                        ON s."OrganizationId" = a."OrganizationId" AND s."DeletedAt" IS NULL
                    WHERE a."Id" = v_adr_id;

                    IF v_status = 1 AND v_score >= v_threshold THEN
                        UPDATE "Adrs" SET "Status" = 2, "UpdatedAt" = now() WHERE "Id" = v_adr_id;
                    ELSIF v_status IN (2, 4) AND v_score < 0 THEN
                        UPDATE "Adrs" SET "Status" = 4, "UpdatedAt" = now() WHERE "Id" = v_adr_id;
                    ELSIF v_status = 4 AND v_score >= 0 THEN
                        UPDATE "Adrs" SET "Status" = 2, "UpdatedAt" = now() WHERE "Id" = v_adr_id;
                    END IF;

                    RETURN NULL;
                END; $$ LANGUAGE plpgsql;

                CREATE TRIGGER trg_adr_votes_recalc
                AFTER INSERT OR UPDATE OR DELETE ON "AdrVotes"
                FOR EACH ROW EXECUTE FUNCTION fn_adr_votes_recalc();
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_adr_votes_recalc ON \"AdrVotes\";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_adr_votes_recalc();");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS change_adr_status(uuid, int, int);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS withdraw_vote_adr(uuid, uuid);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS vote_adr(uuid, uuid, int);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS create_adr(uuid, uuid, uuid, text, jsonb);");
            migrationBuilder.Sql("DROP VIEW IF EXISTS organization_members;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS adr_summary;");
        }
    }
}
