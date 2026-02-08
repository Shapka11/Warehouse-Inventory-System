using Application.Abstractions.Persistence.Repositories;

namespace Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IRollsRepository RollsRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken token = default);
}