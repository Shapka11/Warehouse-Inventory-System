using Application.Contracts.Statistics.Operations;

namespace Application.Contracts.Statistics;

public interface IStatisticsRollService
{
    Task<GetStatisticsRoll.Response> GetStatisticsAsync(GetStatisticsRoll.Request request);
}