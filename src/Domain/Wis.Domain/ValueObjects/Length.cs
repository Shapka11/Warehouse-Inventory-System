namespace Domain.ValueObjects;


public sealed record Length
{
    public Length(double value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Length must be positive.");
        }

        Value = value;
    }

    public double Value { get; }
}