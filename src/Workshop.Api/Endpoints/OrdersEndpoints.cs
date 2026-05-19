using Workshop.Api.Dtos;
using Workshop.Api.Models;
using Workshop.Api.Repositories;

namespace Workshop.Api.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        group.MapGet("/", (IOrderRepository repo) => Results.Ok(repo.GetAll()))
            .WithName("ListOrders");

        group.MapGet("/{id:guid}", (Guid id, IOrderRepository repo) =>
        {
            var order = repo.GetById(id);
            return order is null ? Results.NotFound() : Results.Ok(order);
        }).WithName("GetOrder");

        // EXERCISE 1: this POST handler has no validation.
        group.MapPost("/", (CreateOrderRequest request, IOrderRepository repo) =>
        {
            var order = new Order(Guid.NewGuid(), request.UserId, request.Product, request.Quantity, request.Price);
            repo.Add(order);
            return Results.Created($"/orders/{order.Id}", order);
        }).WithName("CreateOrder");

        return app;
    }
}
