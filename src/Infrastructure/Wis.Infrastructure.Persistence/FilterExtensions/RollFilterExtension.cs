using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.FilterExtensions;

public static class RollFilterExtension
{
    public static IQueryable<RollEntity> FilterById(this IQueryable<RollEntity> query, Guid[]? ids)
    {
        return ids is not null && ids.Length > 0 ? query.Where(r => ids.Contains(r.Id)) : query;
    }

    public static IQueryable<RollEntity> FilterByWeight(
        this IQueryable<RollEntity> query,
        double[]? weights)
    {
        return weights is not null && weights.Length > 0
            ? query.Where(r => weights.Contains(r.Weight))
            : query;
    }

    public static IQueryable<RollEntity> FilterByLength(
        this IQueryable<RollEntity> query,
        double[]? lengths)
    {
        return lengths is not null && lengths.Length > 0
            ? query.Where(r => lengths.Contains(r.Length))
            : query;
    }

    public static IQueryable<RollEntity> FilterByAddedDate(
        this IQueryable<RollEntity> query,
        DateTimeOffset[]? dates)
    {
        return dates is not null && dates.Length > 0
            ? query.Where(r => dates.Contains(r.AddedDate))
            : query;
    }

    public static IQueryable<RollEntity> FilterByRemovedDate(
        this IQueryable<RollEntity> query,
        DateTimeOffset[]? dates)
    {
        return dates is not null && dates.Length > 0
            ? query.Where(r => r.RemovedDate.HasValue && dates.Contains(r.RemovedDate.Value.Date))
            : query;
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