namespace Workshop.Api.Dtos;

public sealed record CreateOrderRequest(Guid UserId, string Product, int Quantity, decimal Price);
