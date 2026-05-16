# AI_RULES.md — Strict Technical and Architectural Rules

Rules for all AI assistants working in the ToolSnap repository.
Rules are derived from actual code patterns, not theory.

---

## General Rules

1. Maintain consistency with the Clean Architecture layering — `Domain → Application → Infrastructure → Api`
2. Prefer extending existing patterns over replacing them
3. Keep methods focused: handlers orchestrate, repos persist, queries read, controllers map
4. Never introduce hidden side effects — every operation must be visible in the call chain
5. No static mutable state; no singleton services with mutable fields

---

## Architecture Rules

### Layer Boundaries (enforced by project references)

```
Domain       → no dependencies (pure C#)
Application  → depends on Domain only
Infrastructure → depends on Application + Domain
Api          → depends on Infrastructure + Domain
```

**Violations are compile errors** — do not attempt to reference Infrastructure from Application or Domain from outside its allowed dependents.

### Business Logic Placement

| Logic Type | Must Live In | Must NOT Live In |
|------------|-------------|-----------------|
| Domain invariants | `Domain/Models/{Entity}.cs` (methods) | Controllers, Handlers |
| Orchestration / workflow | `Application/Entities/*/Commands/*Handler.cs` | Controllers, Repositories |
| Data reads | `Infrastructure/Persistence/Queries/*Queries.cs` | Controllers, Handlers directly |
| HTTP mapping | `Api/Controllers/*.cs` | Handlers, Repos |
| DTO construction | `Api/DTOs/*Dto.FromDomain()` | Handlers, Domain |

### Controller Rules

- Controllers call `_mediator.Send(command)` and map the result. Nothing else.
- No `DbContext`, no `HttpClient`, no service logic in controllers.
- Use `.Match()` on `Either` — never `result.IsRight` conditional checks.

### Data Access Isolation

- Only `Infrastructure/Persistence/` classes may reference `ApplicationDbContext`
- Only `Application/Common/Interfaces/` interfaces are injected into handlers
- Handlers never directly reference `ApplicationDbContext` — only `IApplicationDbContext` for transactions

---

## Naming Conventions

| Construct | Rule | Example |
|-----------|------|---------|
| Classes | PascalCase | `ToolAssignmentRepository` |
| Interfaces | `I` + PascalCase | `IToolAssignmentRepository` |
| Commands | `{Verb}{Entity}Command` | `CreateToolAssignmentCommand` |
| Command handlers | `{Command}Handler` (nested or same file) | `CreateToolAssignmentCommandHandler` |
| Queries (class) | `{Entity}Queries` | `ToolAssignmentQueries` |
| Query interfaces | `I{Entity}Queries` | `IToolAssignmentQueries` |
| Repository interfaces | `I{Entity}Repository` | `IToolAssignmentRepository` |
| Validators | `{Command}Validator` | `CreateToolAssignmentCommandValidator` |
| DTO records | `{Entity}Dto`, `Create{Entity}Dto`, `Update{Entity}Dto` | `ToolDto`, `CreateToolDto` |
| Strong-typed IDs | `{Entity}Id` as `record struct` | `ToolId`, `UserId` |
| EF configs | `{Entity}Configurations` | `ToolConfigurations` |
| Error factories | `{Entity}ErrorFactory` (static extension class) | `ToolErrorFactory` |
| Controller services | `I{Entity}ControllerService` | `IToolControllerService` |
| Private fields | `_camelCase` | `_mediator`, `_context` |
| Constants | `PascalCase` | `JwtSettings.SectionName` |
| Async methods | `{Name}Async` suffix | `GetAllAsync`, `HandleAsync` |

---

## C# Rules

### Async/Await

```csharp
// CORRECT
public async Task<Either<ToolException, Tool>> Handle(
    CreateToolCommand request, CancellationToken cancellationToken)
{
    var tool = Tool.New(...);
    return await _repository.AddAsync(tool, cancellationToken);
}

// WRONG — never block async
var result = _repository.AddAsync(tool, ct).Result;
var result = _repository.AddAsync(tool, ct).GetAwaiter().GetResult();
```

- Every `async` method must have `CancellationToken` as last parameter.
- Always pass `cancellationToken` through — never pass `CancellationToken.None` internally.

### Dependency Injection

```csharp
// CORRECT — primary constructor injection
public class ToolRepository(ApplicationDbContext context) : IToolRepository { }

// WRONG — service locator
public class ToolRepository(IServiceProvider provider)
{
    var context = provider.GetService<ApplicationDbContext>(); // NO
}
```

### Functional Result Types

```csharp
// CORRECT — Either for commands
Either<ToolException, Tool> result = await _mediator.Send(command);
return result.Match<ActionResult<ToolDto>>(
    tool  => Ok(ToolDto.FromDomain(tool)),
    error => error.ToObjectResult()
);

// CORRECT — Option for queries
Option<Tool> tool = await _queries.GetByIdAsync(id, ct);
// then in handler:
return tool.Match(
    some => Either<ToolException, Tool>.Right(some),
    ()   => Either<ToolException, Tool>.Left(new ToolNotFoundException(id))
);

// WRONG — null checks instead of Option
Tool? tool = await context.Tools.FirstOrDefaultAsync(...);
if (tool == null) return NotFound(); // Controllers should not do this
```

### Models Over Dynamics

```csharp
// CORRECT
public record GeminiDetectionResult(string ToolType, string? Brand, double Confidence);

// WRONG
dynamic result = JsonSerializer.Deserialize<dynamic>(json);
```

### Avoid Static Mutable State

```csharp
// WRONG
public static class ToolCache
{
    public static List<Tool> CachedTools = new(); // mutable static — forbidden
}
```

---

## SQL / EF Core Rules

1. **No raw SQL** — all queries via EF Core LINQ or `context.Set<T>()`. No `context.Database.ExecuteSqlRaw()`.
2. **No Dapper** — not a dependency, do not add it.
3. **Parameterized queries only** — EF Core handles this automatically; never interpolate user input into SQL strings.
4. **Naming** — do not add `[Column("name")]` or `[Table("name")]` — `EFCore.NamingConventions` converts automatically.
5. **No Data Annotations on Domain entities** — use Fluent API in `{Entity}Configurations.cs` files.
6. **Transactions for multi-step operations**: use `IApplicationDbContext.BeginTransactionAsync()` — do not use `TransactionScope`.

### N+1 Prevention

```csharp
// WRONG — N+1: loads tools then issues separate query per tool
var tools = await context.Tools.ToListAsync(ct);
foreach (var t in tools)
    _ = t.ToolAssignments.Count; // lazy load triggers N+1

// CORRECT — eager load with Include
var tools = await context.Tools
    .Include(t => t.ToolAssignments)
    .ToListAsync(ct);
```

### Projection for Large Columns

```csharp
// WRONG — loads `content` (raw bytes) even when only file path is needed
var photos = await context.PhotosForDetection.ToListAsync(ct);

// CORRECT — project to exclude large column
var paths = await context.PhotosForDetection
    .Select(p => new { p.Id, p.FilePath, p.FileName })
    .ToListAsync(ct);
```

---

## Refactoring Rules

1. **Refactor incrementally** — one behavior change per commit
2. **Preserve observable behavior** — refactoring must not change API responses or DB writes
3. **Avoid large-scale rewrites** — do not restructure folder layout or rename namespaces without explicit request
4. **Prefer composition** — inject dependencies, do not inherit from base service classes
5. **Before renaming a public interface method** — `Grep` for all usages across all 4 projects first
6. **Do not merge commands** — `CreateToolCommand` and `UpdateToolCommand` are separate records for a reason

---

## Testing Expectations

No test project currently exists. When tests are added:

- Use `xUnit` (standard for .NET)
- Use `Testcontainers` for PostgreSQL integration tests — no in-memory EF databases
- Keep domain unit tests fast and free of infrastructure dependencies
- Test handlers through the full mediator pipeline with a real `ApplicationDbContext`
- Do not mock repositories in unit tests of handlers — integration test with a real DB
- Validators should have dedicated unit tests: one test class per validator

---

## Performance Rules

1. **Avoid unnecessary allocations** — use `IReadOnlyList<T>` return types (no `List<T>` copy)
2. **Watch N+1 queries** — always check whether navigation properties need `Include()`
3. **LINQ materialization** — call `.ToListAsync()` once at the end, never materialize mid-chain
4. **Avoid loading `content` column** from `photos_for_detection` unless reading image bytes is required
5. **Do not use `.ToList()` then LINQ** — keep queries deferred until materialization
6. **Pagination** — for large collections (tools, assignments), consider adding `.Skip().Take()` before loading into memory
7. **Avoid `Count()` followed by `ToList()`** — single DB round trip where possible

---

## Security Rules

1. **JWT secret must not be committed** — `appsettings.json` has a placeholder; real secrets via environment variables or .NET user secrets
2. **`RequireHttpsMetadata` must be `true` in production** — currently `false` in dev config
3. **Never log JWT tokens, passwords, or refresh tokens**
4. **BCrypt is used for passwords** — do not change the hashing algorithm; do not store plain passwords anywhere
5. **CORS is open** (`AllowAnyOrigin`) — acceptable for dev; restrict for production environments
6. **Never expose `PasswordHash`** or `RefreshToken` in any DTO or API response
