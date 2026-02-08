using Application.Contracts.Statistics.Models;

namespace Application.Contracts.Statistics.Operations;

public static class GetStatisticsRoll
{
    public readonly record struct Request(DateTimeOffset From, DateTimeOffset To);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(StatisticsRollDto StatisticsRoll) : Response;

        public sealed record Failure(string Message) : Response;
    }
}