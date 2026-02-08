using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.FilterExtensions;

public static class RollFilterExtension
{
    public static IQueryable<RollEntity> FilterById(this IQueryable<RollEntity> query, Guid[]? ids)
    {
        return ids is null ? query : query.Where(r => ids.Contains(r.Id));
    }

    public static IQueryable<RollEntity> FilterByWeight(
        this IQueryable<RollEntity> query,
        double[]? weights)
    {
        return weights is null ? query : query.Where(r => weights.Contains(r.Weight));
    }
    
    public static IQueryable<RollEntity> FilterByLength(
        this IQueryable<RollEntity> query,
        double[]? lengths)
    {
        return lengths is null ? query : query.Where(r => lengths.Contains(r.Length));
    }
    
    public static IQueryable<RollEntity> FilterByAddedDate(
        this IQueryable<RollEntity> query,
        DateTimeOffset[]? dates)
    {
        return dates is null ? query : query.Where(r => dates.Contains(r.AddedDate));
    }
    
    public static IQueryable<RollEntity> FilterByRemovedDate(
        this IQueryable<RollEntity> query,
        DateTimeOffset[]? dates)
    {
        return dates is null
            ? query
            : query.Where(r => r.RemovedDate.HasValue && dates.Contains(r.RemovedDate.Value.Date));
    }
    
    public static IQueryable<RollEntity> FilterByDateRange(
        this IQueryable<RollEntity> query,
        (DateTimeOffset From, DateTimeOffset To)? range)
    {
        if (range is null) return query;
        var (from, to) = range.Value;

        return query.Where(r => r.AddedDate <= to && (r.RemovedDate == null || r.RemovedDate >= from));
    }
}