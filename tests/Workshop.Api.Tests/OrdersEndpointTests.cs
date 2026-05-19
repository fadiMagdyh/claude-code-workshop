using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Workshop.Api.Dtos;

namespace Workshop.Api.Tests;

public sealed class OrdersEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OrdersEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Get_orders_returns_ok()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/orders");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_orders_with_valid_payload_returns_created()
    {
        var client = _factory.CreateClient();
        var request = new CreateOrderRequest(Guid.NewGuid(), "Widget", 2, 19.99m);

        var response = await client.PostAsJsonAsync("/orders", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    // EXERCISE 1: remove Skip after adding CreateOrderRequestValidator. 
    [Fact(Skip = "Exercise 1: implement CreateOrderRequest validation")]
    public async Task Post_orders_with_negative_price_returns_validation_problem()
    {
        var client = _factory.CreateClient();
        var request = new CreateOrderRequest(Guid.NewGuid(), "Widget", 1, -1.00m);

        var response = await client.PostAsJsonAsync("/orders", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
