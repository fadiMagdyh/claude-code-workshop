namespace Workshop.Api.Models;

public sealed record Order(Guid Id, Guid UserId, string Product, int Quantity, decimal Price);
