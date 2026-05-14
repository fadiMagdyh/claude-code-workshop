using Workshop.Api.Models;

namespace Workshop.Api.Repositories;

public interface IUserRepository
{
    IReadOnlyCollection<User> GetAll();
    User? GetById(Guid id);
    User Add(User user);
}
