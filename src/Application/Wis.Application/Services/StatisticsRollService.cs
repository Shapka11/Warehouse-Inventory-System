using Application.Abstractions.Persistence;
using Application.Abstractions.Persistence.Queries;
using Application.Contracts.Statistics;
using Application.Contracts.Statistics.Models;
using Application.Contracts.Statistics.Operations;

namespace Application.Services;

public sealed class StatisticsRollService : IStatisticsRollService
{
    private readonly IPersistenceContext _context;

    public StatisticsRollService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<GetStatisticsRoll.Response> GetStatisticsAsync(GetStatisticsRoll.Request request)
    {
        var from = request.From;
        var to = request.To;

        var rolls = await _context.RollsRepository
            .QueryAsync(RollsQuery.Build(builder => builder.WithDateRange((from, to))));
        var allRolls = rolls.ToList();

        var addedInPeriod = allRolls.Where(r => r.AddedDate >= from).ToList();
        var removedInPeriod = allRolls.Where(r => r.RemovedDate <= to).ToList();

        var avgWeight = allRolls.Count != 0 ? allRolls.Average(r => r.Weight.Value) : 0;
        var avgLength = allRolls.Count != 0 ? allRolls.Average(r => r.Length.Value) : 0;

        var maxLength = allRolls.Count != 0 ? allRolls.Max(r => r.Length.Value) : 0;
        var minLength = allRolls.Count != 0 ? allRolls.Min(r => r.Length.Value) : 0;

        var maxWeight = allRolls.Count != 0 ? allRolls.Max(r => r.Weight.Value) : 0;
        var minWeight = allRolls.Count != 0 ? allRolls.Min(r => r.Weight.Value) : 0;
        var totalWeight = allRolls.Count != 0 ? allRolls.Sum(r => r.Weight.Value) : 0;

        var storageDurations = removedInPeriod
            .Select(r => r.RemovedDate.Value - r.AddedDate)
            .ToList();

        var minStorageTime = storageDurations.Count != 0 ? storageDurations.Min() : TimeSpan.Zero;
        var maxStorageTime = storageDurations.Count != 0 ? storageDurations.Max() : TimeSpan.Zero;

        var durations = allRolls
            .Where(r => r.RemovedDate.HasValue)
            .Select(r => r.RemovedDate.Value - r.AddedDate)
            .ToList();

        var daysCount = (to.Date - from.Date).Days + 1;
        var dailySnapshots = Enumerable.Range(0, daysCount)
            .Select(offset => from.Date.AddDays(offset))
            .Select(day =>
            {
                var rollsOnStock = allRolls.Where(r => 
                        r.AddedDate.Date <= day && 
                        (r.RemovedDate is null || r.RemovedDate.Value.Date > day))
                    .ToList();

                return new
                {
                    Day = DateOnly.FromDateTime(day),
                    Count = rollsOnStock.Count,
                    TotalWeight = rollsOnStock.Sum(r => r.Weight.Value)
                };
            })
            .ToList();

        if (dailySnapshots.Count == 0) 
        {
            return new GetStatisticsRoll.Response.Failure("No data for this period");
        }

        var maxCountDay = dailySnapshots.MaxBy(s => s.Count);
        var minCountDay = dailySnapshots.MinBy(s => s.Count);

        var maxWeightDay = dailySnapshots.MaxBy(s => s.TotalWeight);
        var minWeightDay = dailySnapshots.MinBy(s => s.TotalWeight);

        var stats = new StatisticsRollDto(
            CountAdded: addedInPeriod.Count,
            CountRemoved: removedInPeriod.Count,
            AverageLength: avgLength,
            AverageWeight: avgWeight,
            MinLength: minLength,
            MaxLength: maxLength,
            MinWeight: minWeight,
            MaxWeight: maxWeight,
            TotalWeight: totalWeight,
            MinStorageTime: minStorageTime,
            MaxStorageTime: maxStorageTime,
            DayWithMaxCount: maxCountDay.Day,
            DayWithMinCount: minCountDay.Day,
            DayWithMaxWeight: maxWeightDay.Day,
            DayWithMinWeight: minWeightDay.Day);

        return new GetStatisticsRoll.Response.Success(stats);
    }
}