using Application.Abstractions.Persistence.Queries;
using Domain.Entities;

namespace Application.Abstractions.Persistence.Repositories;

public interface IRollsRepository
{
    Task AddAsync(Roll roll, CancellationToken token = default);

    Task Update(Roll roll, CancellationToken token = default);

    public Task<IEnumerable<Roll>> QueryAsync(RollsQuery query, CancellationToken token = default);
}