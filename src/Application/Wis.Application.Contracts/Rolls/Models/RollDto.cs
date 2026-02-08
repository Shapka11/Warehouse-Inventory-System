namespace Application.Contracts.Rolls.Models;

public sealed record RollDto(
    Guid id,
    double length,
    double weight,
    DateTimeOffset addedDate,
    DateTimeOffset? removedDate);