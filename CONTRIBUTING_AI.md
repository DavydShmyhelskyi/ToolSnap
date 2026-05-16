# CONTRIBUTING_AI.md — AI Contribution Guidelines

How AI assistants should introduce changes into the ToolSnap repository.
Companion to `CLAUDE.md` (operational) and `AI_RULES.md` (technical rules).

---

## AI Contribution Principles

- **Make minimal safe changes** — touch only what is required to fulfill the request
- **Prefer incremental updates** — one coherent change per session, not sweeping rewrites
- **Explain reasoning before major modifications** — describe what will change and why before editing
- **Preserve architectural consistency** — new code must be indistinguishable in style from existing code
- **Never speculate** — do not implement features that were not explicitly requested

---

## Before Implementing Changes

Complete this checklist before writing any code:

- [ ] Identify the affected entity/module
- [ ] Read the existing entity in `Domain/Models/`
- [ ] Read the existing command handler(s) in `Application/Entities/{Entity}/Commands/`
- [ ] Read the existing repository + query interface in `Application/Common/Interfaces/`
- [ ] Read the existing repository + query implementation in `Infrastructure/Persistence/`
- [ ] Read the existing controller in `Api/Controllers/`
- [ ] Read the existing DTO in `Api/DTOs/`
- [ ] Search for all usages of the symbol being changed: `Grep "{SymbolName}"`
- [ ] Verify downstream usages won't break (serialization, API shape, DI registration)
- [ ] Confirm whether a DB migration is needed

---

## When Adding a New Feature

### Full Entity (new table, new endpoints)

Follow the 13-step sequence in `AI_CONTEXT.md` → "Adding a New Entity". Steps must be completed in order because each step depends on the previous.

Key file pairs to create (example: adding `Warehouse` entity):

| Layer | Files |
|-------|-------|
| Domain | `Domain/Models/Warehouses/Warehouse.cs`, `WarehouseId.cs`, `WarehouseExceptions.cs` |
| Application | `Application/Common/Interfaces/Repositories/IWarehouseRepository.cs`, `Queries/IWarehouseQueries.cs` |
| Application | `Application/Entities/Warehouses/Commands/Create/CreateWarehouseCommand.cs` + Handler + Validator |
| Infrastructure | `Infrastructure/Persistence/Configurations/WarehouseConfigurations.cs` |
| Infrastructure | `Infrastructure/Persistence/Repositories/WarehouseRepository.cs`, `Queries/WarehouseQueries.cs` |
| Infrastructure | Register in `ConfigurePersistenceServices.AddRepositories()` |
| Api | `Api/DTOs/WarehouseDto.cs`, `Api/Modules/Validators/WarehouseValidators.cs` |
| Api | `Api/Modules/Errors/WarehouseErrorFactory.cs` |
| Api | `Api/Controllers/WarehousesController.cs` |
| Api | Register service in `SetupModule.AddControllerServices()` |
| Migration | `dotnet ef migrations add AddWarehouses --project src/Infrastructure --startup-project src/Api` |

### Adding a New Endpoint to Existing Entity

1. Add command/query record + handler in `Application/Entities/{Entity}/Commands/` or `Queries/`
2. Add validator if the command has input
3. Add action method to existing controller
4. Update DTO if response shape changes (add new record to existing `*Dto.cs`)
5. Add error case to existing `*ErrorFactory.cs` if new exceptions are introduced
6. No migration needed unless DB schema changes

### Adding a New Field to Existing Entity

1. Add property to domain entity in `Domain/Models/`
2. Update `Domain` factory method `New()` and `Update()` if applicable
3. Update EF configuration in `Infrastructure/Persistence/Configurations/{Entity}Configurations.cs`
4. Update existing `Create*Command` and `Update*Command` if the field is settable via API
5. Update validators accordingly
6. Update DTO — add the field to `XxxDto`, `CreateXxxDto`, `UpdateXxxDto` as appropriate
7. Update `XxxDto.FromDomain()` to include the new field
8. Add a migration: `dotnet ef migrations add Add{FieldName}To{Entity}`

---

## When Modifying Database Logic

- **Verify transaction scope** — does the operation touch multiple tables? If yes, use `IApplicationDbContext.BeginTransactionAsync()`
- **Preserve column names** — `EFCore.NamingConventions` auto-generates snake_case; do not override with `[Column]`
- **Analyze impact on existing data** — adding a NOT NULL column requires a default value in the migration
- **Check existing migrations** — only one migration exists (`20260221151010_Innitial.cs`); new migrations are additive
- **Never edit existing migration files** — create a new migration instead
- **EF migration command** (always specify both projects):
  ```
  dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api --output-dir Persistence/Migrations
  ```

---

## When Modifying Authentication Logic

Authentication is a high-risk area. Extra caution required:

- `JwtTokenGenerator` generates both access and refresh tokens — changes affect all clients
- `User.SetRefreshToken()` / `User.RevokeRefreshToken()` — changes must maintain the entity method contract
- JWT claim names (`role_id`, `is_active`, `email_confirmed`) are part of the token contract — renaming breaks existing tokens
- `[Authorize]` attribute is on individual actions — verify additions/removals with the team
- The `ClockSkew: TimeSpan.Zero` setting means tokens expire exactly on time — do not increase this without understanding the security tradeoff

---

## When Modifying the Gemini AI Integration

`src/Api/Services/GemeniAiService/GeminiService.cs`:

- API key is read directly from `IConfiguration` — any rename of the config key breaks the service
- The model is hardcoded to `gemini-2.5-flash-lite` — changes require testing against the new model
- The response JSON schema is embedded in the prompt — schema changes must be reflected in the deserialization models
- `DetectionPromptBuilder` constructs the prompt using brand/model/type data from DB — changes to entity structure cascade here

---

## Pull Request Guidelines

AI-generated PRs must include a description covering:

### Summary
- What changed (which entities, commands, endpoints)
- Why the change was made

### Affected Modules
List all projects and significant files modified:
- `Domain` — entity changes
- `Application` — new commands/validators/interfaces
- `Infrastructure` — new repos/queries/configs/migrations
- `Api` — new endpoints/DTOs/error factories

### Risks
- Does this change the API contract (DTO shape, endpoint URL, HTTP method)?
- Does this require a migration?
- Does this affect authentication or authorization?
- Does this change behavior for existing data?

### Backward Compatibility
- Are existing endpoints preserved?
- Are existing DTO fields preserved (no renames)?
- Are existing DB columns preserved?

### Migration Notes
If a migration is included:
- Name and purpose of the migration
- Whether it is safe to apply to a DB with existing data
- Whether any default values were provided for new NOT NULL columns

---

## Dangerous Areas (Extra Caution Required)

| Area | Why Dangerous | What to Check |
|------|--------------|---------------|
| `Application/Common/Interfaces/` | Interfaces implemented by multiple classes; changes cascade | All implementations must be updated |
| `Api/DTOs/` | DTOs are serialized to JSON; field renames break API consumers | Check all controller actions that return the DTO |
| `JwtTokenGenerator.cs` | Token claims are parsed by clients; changes invalidate existing tokens | Do not rename existing claim keys |
| `User` entity | Refresh token and password hash are security-critical | No plain-text storage; no DTO exposure |
| `LocalFileStorage` | Writes to filesystem; path errors cause data loss | Validate paths before write; do not change the `uploads/` root without updating all callers |
| `ApplicationDbContext.cs` | Removing a `DbSet<T>` drops table awareness from EF | Always add, never remove, unless migration also drops the table intentionally |
| `ConfigurePersistenceServices.cs` | Missing DI registration causes runtime `NullReferenceException` | After adding repo/query, always register both concrete + interface |
| Middleware pipeline in `Program.cs` | Order of `UseAuthentication/UseAuthorization/UseCors` is critical | Never reorder without understanding the implications |
| `AuthController` vs `AuthentificationController` | Duplicate controllers exist — unclear which is authoritative | Investigate before modifying either |

---

## Preferred AI Behavior

1. **Think before modifying** — re-read the existing implementation, not just the request
2. **Analyze dependencies first** — use Grep to find all usages of symbols you intend to change
3. **Avoid speculative refactoring** — do not clean up unrelated code while fixing a bug
4. **Preserve existing architecture style** — new code must match indentation, naming, and patterns of surrounding code
5. **One concern per change** — do not mix a bug fix with a refactor in the same edit
6. **Validate DI completeness** — after adding any new service, verify it is registered in the correct `ConfigureServices` method
7. **State uncertainty explicitly** — if the intent of a piece of code is unclear, say so before proceeding
