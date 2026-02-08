namespace Domain.Entities.ResultTypes;

public abstract record RollRemoveResult
{
    private RollRemoveResult() { }

    public sealed record Success() : RollRemoveResult;

    public sealed record Failure(string Error) : RollRemoveResult;
}