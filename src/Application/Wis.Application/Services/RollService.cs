using Application.Abstractions.Persistence;
using Application.Abstractions.Persistence.Queries;
using Application.Contracts.Rolls;
using Application.Contracts.Rolls.Operations;
using Application.Mapping;
using Domain.Entities;
using Domain.Entities.ResultTypes;
using Domain.ValueObjects;

namespace Application.Services;

public sealed class RollService : IRollService
{
    private readonly IPersistenceContext _context;

    public RollService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<AddRoll.Response> AddRollAsync(AddRoll.Request request)
    {
        var length = new Length(request.Length);
        var weight = new Weight(request.Weight);
        var roll = new Roll(Guid.NewGuid(), length, weight, DateTimeOffset.UtcNow);

        await _context.RollsRepository.AddAsync(roll);
        await _context.SaveChangesAsync();

        return new AddRoll.Response(roll.MapToDto());
    }

    public async Task<RemoveRoll.Response> RemoveRollAsync(RemoveRoll.Request request)
    {
        var id = request.Id;

        var rolls = await _context.RollsRepository
            .QueryAsync(RollsQuery.Build(builder => builder.WithId(id)));
        var roll = rolls.SingleOrDefault();

        if (roll is null)
        {
            return new RemoveRoll.Response.Failure("Roll not found");
        }

        RollRemoveResult removeResult = roll.Remove();
        if (removeResult is RollRemoveResult.Failure failure)
        {
            return new RemoveRoll.Response.Failure(failure.Error);
        }
        
        await _context.RollsRepository.Update(roll);
        await _context.SaveChangesAsync();

        return new RemoveRoll.Response.Success(roll.MapToDto());
    }

    public async Task<GetRollsList.Response> GetRollsListAsync(GetRollsList.Request request)
    {
        var filter = RollsQuery.Build(builder =>
        {
            if (request.Id.HasValue)
                builder.WithId(request.Id.Value);

            if (request.Length.HasValue)
                builder.WithLength(request.Length.Value);

            if (request.Weight.HasValue)
                builder.WithWeight(request.Weight.Value);

            if (request.AddedDate.HasValue)
                builder.WithAddedDate(request.AddedDate.Value);

            if (request.RemovedDate.HasValue)
                builder.WithRemovedDate(request.RemovedDate.Value);

            return builder;
        });

        IEnumerable<Roll> rolls = await _context.RollsRepository.QueryAsync(filter);

        return new GetRollsList.Response.Success(rolls.MapToDto());
    }
}