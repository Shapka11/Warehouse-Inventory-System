using Application.Abstractions.Persistence;
using Application.Abstractions.Persistence.Repositories;

namespace Infrastructure.Persistence;

public sealed class PersistenceContext : IPersistenceContext
{
    private readonly WarehouseDbContext _db;

    public PersistenceContext(
        WarehouseDbContext db,
        IRollsRepository rollsRepository)
    {
        _db = db;
        RollsRepository = rollsRepository;
    }

    public IRollsRepository RollsRepository { get; }

    public Task<int> SaveChangesAsync(CancellationToken token = default)
        => _db.SaveChangesAsync(token);
}