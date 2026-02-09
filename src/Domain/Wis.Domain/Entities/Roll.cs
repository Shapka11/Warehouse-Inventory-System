using Domain.Entities.ResultTypes;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Roll
{
    public Roll(
        Guid id,
        Length length,
        Weight weight,
        DateTimeOffset addedDate,
        DateTimeOffset? removedDate = null)
    {
        Id = id;
        Length = length;
        Weight = weight;
        AddedDate = addedDate;
        RemovedDate = removedDate;
    }

    public Guid Id { get; }

    public Length Length { get; }

    public Weight Weight { get; }

    public DateTimeOffset AddedDate { get; }

    public DateTimeOffset? RemovedDate { get; private set; }

    public RollRemoveResult Remove()
    {
        if (RemovedDate.HasValue)
        {
            return new RollRemoveResult.Failure("Roll already removed.");
        }

        RemovedDate = DateTimeOffset.UtcNow;

        return new RollRemoveResult.Success();
    }
}