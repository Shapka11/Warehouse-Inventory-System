using Application.Abstractions.Persistence.Queries;
using Application.Abstractions.Persistence.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.FilterExtensions;
using Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class RollsRepository : IRollsRepository
{
    private readonly WarehouseDbContext _context;

    public RollsRepository(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Roll roll, CancellationToken token = default)
    {
        await _context.AddAsync(roll.MapToEntity(), token);
    }
    
    public Task Update(Roll roll, CancellationToken token = default)
    {
        var newEntity = roll.MapToEntity();
        var localEntity = _context.Rolls.Local.FirstOrDefault(e => e.Id == newEntity.Id);

        if (localEntity is not null)
        {
            _context.Entry(localEntity).State = EntityState.Detached;
        }

        _context.Rolls.Update(newEntity);

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Roll>> QueryAsync(RollsQuery filter, CancellationToken token = default)
    {
        var entities = await _context.Rolls
            .AsNoTracking()
            .FilterById(filter.Id)
            .FilterByLength(filter.Length)
            .FilterByWeight(filter.Weight)
            .FilterByAddedDate(filter.AddedDate)
            .FilterByRemovedDate(filter.RemovedDate)
            .FilterByDateRange(filter.DateRange)
            .ToListAsync(token);

        return entities.Select(r => r.MapToDomain());
    }
    
}