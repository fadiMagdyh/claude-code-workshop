using Workshop.Api.Models;

namespace Workshop.Api.Repositories;

public interface IOrderRepository
{
    IReadOnlyCollection<Order> GetAll();
    Order? GetById(Guid id);
    Order Add(Order order);
}
