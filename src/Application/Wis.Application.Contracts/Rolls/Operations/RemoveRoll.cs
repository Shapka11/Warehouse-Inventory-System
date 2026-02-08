using Application.Contracts.Rolls.Models;

namespace Application.Contracts.Rolls.Operations;

public static class RemoveRoll
{
    public readonly record struct Request(Guid Id);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(RollDto Roll) : Response;

        public sealed record Failure(string Message) : Response;
    }
}