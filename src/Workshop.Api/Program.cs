using FluentValidation;
using Workshop.Api.Endpoints;
using Workshop.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePages();

app.MapHealthEndpoints();
app.MapUsersEndpoints();
app.MapOrdersEndpoints();

app.Run();

public partial class Program;
