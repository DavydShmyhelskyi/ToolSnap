# CLAUDE.md — Operational Instructions for Claude Code

This file governs how Claude Code should behave when working in this repository.
See `AI_CONTEXT.md` for full architectural documentation.

---

## Project Understanding

**ToolSnap** is a tool inventory management API built with:
- **ASP.NET Core 10** / **Clean Architecture** / **CQRS** (MediatR)
- **PostgreSQL** via Entity Framework Core 10 + Npgsql
- **LanguageExt** for functional error handling (`Either`, `Option`)
- **Google Gemini AI** for photo-based tool detection

The four layers are `Domain → Application → Infrastructure → Api`.
Each layer is a separate `.csproj`; dependencies only flow inward.

---

## How To Work In This Repository

### Safe Exploration

```
src/Domain/Models/         ← Entity definitions + strong-typed IDs
src/Application/Entities/  ← Commands, validators (one folder per entity)
src/Application/Common/    ← Interfaces for repos/queries, settings, behaviors
src/Infrastructure/Persistence/  ← EF config, repos, queries, migrations
src/Api/Controllers/       ← REST endpoints
src/Api/DTOs/              ← Request/response models
src/Api/Modules/           ← DI wiring (SetupModule), error factories, validators
```

### Most Important Folders

| Priority | Folder | Why |
|----------|--------|-----|
| Critical | `src/Infrastructure/Persistence/` | Schema, repos, migrations |
| Critical | `src/Application/Common/Interfaces/` | Contracts for everything |
| High | `src/Api/Modules/` | DI registration, error mapping |
| High | `src/Application/Entities/` | All business command logic |
| High | `src/Api/Controllers/` | Endpoint definitions |
| Medium | `src/Domain/Models/` | Entity behavior |

### High-Risk Areas

- **`AuthController` vs `AuthentificationController`** — duplicate; verify before touching either
- **`GeminiService`** — reads API key directly from `IConfiguration`, no typed settings
- **`LocalFileStorage`** — writes to relative `uploads/` directory, no validation
- **`User.RefreshToken`** — stored on user entity; concurrent requests can corrupt it
- **`Program.cs` middleware order** — changing order breaks auth/CORS/validation

---

## Coding Expectations

- **Follow existing patterns before introducing anything new.** Every entity follows the same 13-step pattern (see `AI_CONTEXT.md` → "Adding a New Entity").
- **Prefer consistency over cleverness.** The codebase is intentionally uniform.
- **Reuse existing services/helpers.** Check `Application/Common/` for interfaces before creating new abstractions.
- **Avoid unnecessary refactors.** A bug fix does not warrant surrounding cleanup.
- **Preserve backward API compatibility** unless explicitly told otherwise.
- **No half-implemented features.** If a command handler is added, the validator, DTO, controller action, and error factory must also be added.

---

## C#/.NET Guidelines

### Async/Await

- All public methods interacting with DB or I/O must be `async Task<T>` with `CancellationToken cancellationToken` parameter.
- Never block: no `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- Pass `cancellationToken` through every call chain — it is on every existing handler.

### Dependency Injection

- Use constructor injection only. No `IServiceLocator`, no `ServiceProvider.GetService()` in business logic.
- Lifetime rules: DbContext/Repos/Queries → `Scoped`, MediatR behaviors → `Transient`, Settings → `Singleton`.
- Register new repositories in `ConfigurePersistenceServices.AddRepositories()` — both the concrete class and the interface.
- Register new API services in `SetupModule.AddControllerServices()`.

### Service/Repository Pattern

```csharp
// Repository — write operations only
public class BrandRepository(ApplicationDbContext context) : IBrandRepository
{
    public async Task<Brand> AddAsync(Brand entity, CancellationToken ct) { ... }
    public async Task<Brand> UpdateAsync(Brand entity, CancellationToken ct) { ... }
    public async Task<Brand> DeleteAsync(Brand entity, CancellationToken ct) { ... }
}

// Query — read operations only, returns Option<T> or IReadOnlyList<T>
public class BrandQueries(ApplicationDbContext context) : IBrandQueries
{
    public async Task<IReadOnlyList<Brand>> GetAllAsync(CancellationToken ct) { ... }
    public async Task<Option<Brand>> GetByIdAsync(BrandId id, CancellationToken ct) { ... }
}
```

### DTO / Entity Separation

- Entities live in `Domain/` — never expose them directly from controllers.
- DTOs live in `Api/DTOs/` — always use `XxxDto.FromDomain(entity)` for conversion.
- Commands are `record` types in `Application/Entities/*/Commands/` — not DTOs.
- Controller receives DTO → maps to command → sends to mediator → maps result to DTO.

### LINQ

- Prefer LINQ-to-EF (deferred) over `ToList()` then LINQ. Materialize only at the last step.
- Avoid `Select(x => x)` or unnecessary projections in query classes.
- Use `.FirstOrDefaultAsync()` then wrap in `Option<T>`, not `.SingleOrDefaultAsync()` unless uniqueness is guaranteed.

### Exception Handling

- Domain exceptions extend `abstract class {Entity}Exception : Exception`.
- Handlers catch specific exceptions and return `Either.Left(exception)`.
- Controllers never catch exceptions — they use `.Match()` on the returned `Either`.
- Do not throw from validators — return `ValidationFailure` via FluentValidation.

### Logging

- Logging is present in `ApplicationDbContextInitialiser` but sparse elsewhere.
- When adding logging, inject `ILogger<T>` via constructor. Use structured logging: `_logger.LogInformation("Created {EntityType} {EntityId}", ...)`.
- Never log passwords, tokens, or PII.

### Validation

- **DTO validators**: `AbstractValidator<CreateXxxDto>` in `src/Api/Modules/Validators/` — run by `ValidationFilter` before mediator.
- **Command validators**: `AbstractValidator<CreateXxxCommand>` in `src/Application/Entities/*/Commands/` — run by `ValidationBehaviour` in MediatR pipeline.
- Both levels exist and must be maintained.

---

## Database Rules

### SQL / EF Core

- All DB access goes through EF Core — no raw SQL or Dapper.
- Column and table names are auto-converted to snake_case by `EFCore.NamingConventions` — do not add `[Column]` attributes.
- All entity configurations use Fluent API in `Infrastructure/Persistence/Configurations/` — no Data Annotations on domain entities.

### Migrations

```powershell
# Always specify both projects
dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api --output-dir Persistence/Migrations
```

- Never edit migration files after creation.
- Never delete and recreate existing migrations if data may exist.
- Migration runs automatically on startup — no manual `dotnet ef database update` needed in dev.

### Transaction Handling

- For single-entity operations: `repository.AddAsync()` → `SaveChangesAsync()` inside repo is sufficient.
- For multi-entity operations: inject `IApplicationDbContext`, call `BeginTransactionAsync()`, commit/rollback explicitly.
- Do not open transactions for simple single-entity CRUD.

### Performance-Sensitive Areas

- `photos_for_detection.content` stores raw image bytes — avoid loading this column unless needed (use projection).
- Avoid loading `Tool` with all navigation properties when only status is needed.
- `ToolAssignment` queries involving `is_active` filtering are common — ensure index exists on that column.

---

## Safe Modification Strategy

Before changing any code:

1. **Understand the full call chain** — trace from controller → mediator → handler → repo → DB
2. **Search for all usages** — use `Grep` for the symbol name across the solution
3. **Preserve existing contracts** — especially interface methods and DTO shapes
4. **Avoid breaking serialization** — `record` DTOs are serialized to JSON; renaming properties changes the API contract
5. **Avoid changing shared abstractions** — interfaces in `Application/Common/Interfaces/` are implemented by multiple classes

Changing an interface method signature requires updating:
- The interface definition
- All implementations (repos/queries)
- All command handlers that call it
- Potentially DTO mappings

---

## Preferred Workflow

1. **Analyze first** — read the relevant entity files, handler, repository, and controller
2. **Explain findings** — describe what exists before proposing changes
3. **Propose approach** — state exactly which files will change and why
4. **Modify minimally** — touch only what is required
5. **Verify build consistency** — ensure interface implementations are complete and DI registrations are present

---

## Forbidden Behaviors

- **Do not rewrite unrelated code** while fixing a specific issue
- **Do not rename widely-used interfaces or symbols** without tracing all usages
- **Do not introduce new frameworks** (Dapper, Mediator alternatives, etc.) without strong justification
- **Do not replace `Either`/`Option` pattern** with exceptions or nullable — the functional style is intentional
- **Do not generate mock/stub logic in production code** — handlers must be real implementations
- **Do not commit JWT secrets** or real credentials — `appsettings.json` should contain only placeholders
- **Do not bypass `ValidationFilter`** by removing `[ApiController]` or changing filter registration
- **Do not change the middleware pipeline order** in `Program.cs` without understanding auth implications
