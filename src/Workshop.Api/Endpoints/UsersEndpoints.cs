using FluentValidation;
using Workshop.Api.Dtos;
using Workshop.Api.Models;
using Workshop.Api.Repositories;

namespace Workshop.Api.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapGet("/", (IUserRepository repo) => Results.Ok(repo.GetAll()))
            .WithName("ListUsers");

        group.MapGet("/{id:guid}", (Guid id, IUserRepository repo) =>
        {
            var user = repo.GetById(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        }).WithName("GetUser");

        group.MapPost("/", async (
            CreateUserRequest request,
            IValidator<CreateUserRequest> validator,
            IUserRepository repo) =>
        {
            var result = await validator.ValidateAsync(request).ConfigureAwait(false);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            var user = new User(Guid.NewGuid(), request.Name, request.Email);
            repo.Add(user);
            return Results.Created($"/users/{user.Id}", user);
        }).WithName("CreateUser");

        return app;
    }
}
