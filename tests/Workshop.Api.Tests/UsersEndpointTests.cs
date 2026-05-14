using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Workshop.Api.Dtos;
using Workshop.Api.Models;

namespace Workshop.Api.Tests;

public sealed class UsersEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UsersEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Post_users_with_valid_payload_returns_created()
    {
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("Ada Lovelace", "ada@example.com");

        var response = await client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_users_with_invalid_email_returns_validation_problem()
    {
        var client = _factory.CreateClient();
        var request = new CreateUserRequest("Ada", "not-an-email");

        var response = await client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_users_returns_ok()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_then_get_user_round_trips()
    {
        var client = _factory.CreateClient();
        var post = await client.PostAsJsonAsync("/users", new CreateUserRequest("Grace", "grace@example.com"));
        var created = await post.Content.ReadFromJsonAsync<User>();

        var get = await client.GetFromJsonAsync<User>($"/users/{created!.Id}");

        Assert.Equal(created.Id, get?.Id);
    }
}
