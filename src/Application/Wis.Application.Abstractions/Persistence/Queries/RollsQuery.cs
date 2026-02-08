using SourceKit.Generators.Builder.Annotations;

namespace Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public sealed partial record RollsQuery(
    Guid[]? Id,
    double[]? Length,
    double[]? Weight,
    DateTimeOffset[]? AddedDate,
    DateTimeOffset[]? RemovedDate,
    (DateTimeOffset From, DateTimeOffset To)? DateRange);