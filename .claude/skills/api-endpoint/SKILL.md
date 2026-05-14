---
name: api-endpoint
description: Use when adding a new minimal API endpoint to Workshop.Api. Covers route registration, DTOs, FluentValidation validator, and a matching xUnit integration test, following the patterns in UsersEndpoints.cs.
---

# Scaffolding a new endpoint in Workshop.Api

This codebase is .NET 8 minimal APIs with FluentValidation and `WebApplicationFactory`-driven integration tests. Use the `Users` pipeline as the reference shape — it has every layer wired correctly.

## Checklist

When adding a resource named `Foo` with a `POST /foos` create endpoint, produce **exactly** these files:

1. `src/Workshop.Api/Models/Foo.cs` — `public sealed record Foo(Guid Id, ...);`
2. `src/Workshop.Api/Dtos/CreateFooRequest.cs` — `public sealed record CreateFooRequest(...);` (no `Id` field; the endpoint mints it).
3. `src/Workshop.Api/Repositories/IFooRepository.cs` — interface with `GetAll`, `GetById(Guid)`, `Add(Foo)`.
4. `src/Workshop.Api/Repositories/InMemoryFooRepository.cs` — `ConcurrentDictionary<Guid, Foo>`-backed, sealed.
5. `src/Workshop.Api/Validators/CreateFooRequestValidator.cs` — `AbstractValidator<CreateFooRequest>`. Picked up automatically by `AddValidatorsFromAssemblyContaining<Program>()` — do not register it manually.
6. `src/Workshop.Api/Endpoints/FoosEndpoints.cs` — static class with `MapFoosEndpoints(this IEndpointRouteBuilder)`. Use `MapGroup("/foos").WithTags("Foos")`. Inject `IValidator<CreateFooRequest>` into the POST handler and return `Results.ValidationProblem(result.ToDictionary())` on failure, `Results.Created(...)` on success.
7. `tests/Workshop.Api.Tests/FoosEndpointTests.cs` — `IClassFixture<WebApplicationFactory<Program>>`. At minimum: `Get_foos_returns_ok`, `Post_foos_with_valid_payload_returns_created`, `Post_foos_with_<invalid-thing>_returns_validation_problem`.

## Wire-in steps

After creating the files above:

1. Register the repository in `Program.cs` next to the others:
   ```csharp
   builder.Services.AddSingleton<IFooRepository, InMemoryFooRepository>();
   ```
2. Call `app.MapFoosEndpoints();` in `Program.cs` next to the other `Map…Endpoints()` calls.

## Reference files to read first

- `src/Workshop.Api/Endpoints/UsersEndpoints.cs` — canonical endpoint group shape with validation.
- `src/Workshop.Api/Validators/CreateUserRequestValidator.cs` — canonical validator shape.
- `tests/Workshop.Api.Tests/UsersEndpointTests.cs` — canonical integration-test shape.

## Conventions to obey (from CLAUDE.md)

- File-scoped namespaces. `sealed record` for DTOs/models, `sealed class` for repositories.
- `Results.Created($"/foos/{id}", entity)` — never raw `201` writes.
- Test method naming: `Subject_under_test_behavior_returns_expected`.
- No `Task.Result`, no `.Wait()`. Every IO call is awaited.
- No MVC controllers. No `[ApiController]`. Minimal API only.

## Verify

After scaffolding, run:

```powershell
dotnet build
dotnet test --nologo --verbosity quiet
```

Both must be clean before declaring the endpoint done. Warnings are errors in this project.
