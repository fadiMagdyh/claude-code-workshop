using System.Collections.Concurrent;
using Workshop.Api.Models;

namespace Workshop.Api.Repositories;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public IReadOnlyCollection<Order> GetAll() => _orders.Values.ToList();

    public Order? GetById(Guid id) => _orders.TryGetValue(id, out var order) ? order : null;

    public Order Add(Order order)
    {
        _orders[order.Id] = order;
        return order;
    }
}
