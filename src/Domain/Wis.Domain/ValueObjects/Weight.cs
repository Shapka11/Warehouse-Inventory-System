namespace Domain.ValueObjects;

public sealed record Weight
{
    public Weight(double value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Weight must be positive.");
        }

        Value = value;
    }

    public double Value { get; }
}