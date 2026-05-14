using System.Collections.Concurrent;
using Workshop.Api.Models;

namespace Workshop.Api.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public IReadOnlyCollection<User> GetAll() => _users.Values.ToList();

    public User? GetById(Guid id) => _users.TryGetValue(id, out var user) ? user : null;

    public User Add(User user)
    {
        _users[user.Id] = user;
        return user;
    }
}
