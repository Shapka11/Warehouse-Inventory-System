using Application.Contracts.Rolls.Models;

namespace Application.Contracts.Rolls.Operations;

public static class GetRollsList
{
    public readonly record struct Request(
        Guid? Id,
        double? Length,
        double? Weight,
        DateTimeOffset? AddedDate,
        DateTimeOffset? RemovedDate);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(RollsListDto Rolls) : Response;

        public sealed record Failure(string Message) : Response;
    }
}