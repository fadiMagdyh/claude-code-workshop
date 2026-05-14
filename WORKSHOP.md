# Workshop.Api — hands-on workshop

A small but real .NET 8 minimal API used as the working repo for the **Building with Claude Code** workshop. The codebase has one deliberate gap (no validation on `POST /orders`) so you have something real to fix during the live demo.

## Setup

```powershell
dotnet restore
dotnet test
dotnet run --project src/Workshop.Api
```

Swagger UI is available at the URL printed on startup (Development environment only).

## Exercises

The three exercises map to slides 8, 12, and 21 of the deck.

### Exercise 1 — Add validation to `POST /orders` (slide 8)

`Endpoints/OrdersEndpoints.cs` accepts any payload — negative prices, zero quantities, empty product names all return `201 Created`. There is a skipped test in `OrdersEndpointTests.cs` that pins the expected behavior.

**Goal:** add a `CreateOrderRequestValidator` (FluentValidation), wire it in like `UsersEndpoints` does, then remove the `Skip` on `Post_orders_with_negative_price_returns_validation_problem` and make it pass.

**Expected outcome:**
- New file `src/Workshop.Api/Validators/CreateOrderRequestValidator.cs`.
- Rules: `Product` not empty, `Quantity > 0`, `Price >= 0`, `UserId != Guid.Empty`.
- `Post_orders_with_negative_price_returns_validation_problem` no longer skipped, returns `400 Bad Request` with a problem-details body.
- `dotnet test` is fully green with one more test running than before.

### Exercise 2 — Extend `CLAUDE.md` (slide 12)

`CLAUDE.md` covers the basics, but it does not document the **endpoint group pattern** (`MapGroup(...).WithTags(...)`), or the fact that **integration tests share singleton repositories within the same `WebApplicationFactory`** (so test ordering and isolation matter).

**Goal:** add a short "Patterns" section to `CLAUDE.md` capturing those two facts plus anything else you discover while doing Exercise 1.

**Expected outcome:** updated `CLAUDE.md`, ~10 additional lines, no marketing prose — rules you'd want Claude to follow next time.

### Exercise 3 — Add an MCP server OR write a custom skill (slide 21)

Pick one:

**Option A — MCP:** the `.claude/settings.json` file ships with a read-only filesystem MCP server. Add a second MCP — for example, the `git` MCP (`uvx mcp-server-git`) scoped to this repo, or a `sqlite` MCP pointing at a local DB you create. Confirm it loads by running `/mcp` in Claude Code.

**Option B — Skill:** the repo ships with one skill, `.claude/skills/api-endpoint/SKILL.md`. Write a second skill, `repository`, that documents how to add a new in-memory repository following the `InMemoryUserRepository` pattern (interface + sealed class + DI registration in `Program.cs`).

**Expected outcome:**
- If MCP: a second entry in `.claude/settings.json` under `mcpServers`, visible in `/mcp` output.
- If skill: a new `.claude/skills/<name>/SKILL.md` with YAML frontmatter and trigger description. Try invoking it via the Skill tool to confirm it loads.
