# Workshop.Api — Claude Code guide

## Stack
- .NET 8, ASP.NET Core minimal APIs (no controllers).
- xUnit + Microsoft.AspNetCore.Mvc.Testing for integration tests via `WebApplicationFactory<Program>`.
- FluentValidation 11.x for request validation. Validators auto-registered from `Workshop.Api` assembly.
- In-memory repositories (singleton, `ConcurrentDictionary`-backed). No EF / no DB.

## Project layout
- `src/Workshop.Api/` — API. Endpoints live in `Endpoints/*Endpoints.cs` as `MapXEndpoints` extension methods called from `Program.cs`.
- `src/Workshop.Api/{Models,Dtos,Repositories,Validators}/` — one type per file.
- `tests/Workshop.Api.Tests/` — one test class per endpoint group, matching file name.

## Conventions
- File-scoped namespaces. `var` for locals when the type is obvious from the right-hand side. `sealed` on records and concrete classes that aren't designed for inheritance.
- Async: every IO-bound method is `async`/`await`. Use `.ConfigureAwait(false)` in library/endpoint code.
- Naming: PascalCase for types/members, `_camelCase` for private fields, `camelCase` for locals/parameters. Test methods: `Subject_under_test_behavior_returns_expected`.

## Error handling
- Validation failures return RFC 7807 problem details via `Results.ValidationProblem(...)` (400).
- Missing resources return `Results.NotFound()` (404). `app.UseStatusCodePages()` is wired up.

## Test style
- Arrange / Act / Assert with blank lines between sections. One logical assert per test where practical.
- Integration tests only — no mocking of repositories. Each test gets a fresh `WebApplicationFactory<Program>` via `IClassFixture`.

## Never do
- `Task.Result` or `.Wait()` — always `await`. Blocking on async deadlocks the test host.
- Throwing exceptions to signal validation — return `Results.ValidationProblem`.
- Adding `[ApiController]` / MVC controllers — this is a minimal-API codebase.
- Suppressing analyzer warnings — `TreatWarningsAsErrors=true`; fix the root cause.

## Run
- `dotnet restore`
- `dotnet build` (warnings are errors)
- `dotnet test --nologo --verbosity quiet`
- `dotnet run --project src/Workshop.Api` (default URL printed on startup)
