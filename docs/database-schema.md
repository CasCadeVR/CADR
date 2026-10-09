# Схема базы данных CADR

CADR использует PostgreSQL (EF Core, code-first). Все сущности мягко удаляются
(`DeletedAt`, запросы фильтруются через `NotDeletedAt()`), у всех таблиц есть
`CreatedAt` / `UpdatedAt`.

```mermaid
erDiagram
    Organization ||--o{ UserOrganization : members
    Organization ||--o{ UserInvite : invites
    Organization ||--o{ Adr : contains
    Organization ||--o{ AdrFolder : contains
    Organization ||--o{ AdrTemplate : owns
    Organization ||--o{ AdrOrganizationSettings : has

    User ||--o{ UserOrganization : belongs_to
    User ||--o{ UserInvite : invited_as
    User ||--o{ Adr : authors
    User ||--o{ AdrComment : writes
    User ||--o{ AdrVote : votes

    AdrFolder ||--o{ AdrFolder : subfolder_of
    AdrFolder ||--o{ Adr : contains

    Adr ||--o{ AdrSection : has
    Adr ||--o{ AdrComment : has
    Adr ||--o{ AdrVote : has
    Adr ||--o{ AdrLink : "link_source"
    Adr ||--o{ AdrLink : "link_target"

    AdrTemplate ||--o{ AdrTemplateSection : groups

    Organization {
        guid id PK
        string name
        int likesRequiredForApproval
    }

    User {
        guid id PK
        string name
        string login UK
        string email
        string passwordHash
    }

    UserOrganization {
        guid id PK
        guid userId FK
        guid organizationId FK
        enum role "User | Architect | Admin"
    }

    UserInvite {
        guid id PK
        guid userId FK
        guid organizationId FK
        enum role
        enum status "Pending | Accepted | Declined"
    }

    Adr {
        guid id PK
        guid organizationId FK
        guid parentAdrFolderId FK "null = root"
        guid authorId FK
        guid templateId FK "null = no template"
        int number "unique per organization"
        string title
        enum status "Draft | Proposed | Approved | Rejected | NeedsRevision | Deprecated"
    }

    AdrSection {
        guid id PK
        guid adrId FK
        int position
        string title
        string content
    }

    AdrFolder {
        guid id PK
        guid organizationId FK
        guid parentAdrFolderId FK "null = root, self-FK"
        string name "unique per parent"
    }

    AdrComment {
        guid id PK
        guid adrId FK
        guid authorId FK
        string text
    }

    AdrVote {
        guid id PK
        guid adrId FK
        guid userId FK "unique per adr, no self-vote"
        enum value "Like | Dislike"
    }

    AdrLink {
        guid id PK
        guid sourceAdrId FK
        guid targetAdrId FK
        enum type "RelatedTo | Supersedes | DeprecatedBy"
    }

    AdrTemplate {
        guid id PK
        guid organizationId FK "null = built-in"
        bool isBuiltIn
        string name
    }

    AdrTemplateSection {
        guid id PK
        guid templateId FK
        int position
        string title
        string hint
        string placeholder
    }

    AdrOrganizationSettings {
        guid id PK
        guid organizationId FK
        int likesRequiredForApproval
    }
```

## Ключевые бизнес-правила на уровне данных
- **Автоодобрение**: когда счёт голосов Proposed-ADR достигает
  `LikesRequiredForApproval`, ADR автоматически переходит в Approved.
- **Удаление папки каскадное**: удаляются все вложенные подпапки и их ADR.
- **Обновление ADR** заменяет все секции (delete + recreate по Position).
- **Обновление папки** валидируетparent: не сам в себя, не потомок,
  та же организация.