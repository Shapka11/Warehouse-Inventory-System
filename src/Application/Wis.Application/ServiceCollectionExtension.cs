using Application.Contracts.Rolls;
using Application.Contracts.Statistics;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IRollService, RollService>();
        collection.AddScoped<IStatisticsRollService, StatisticsRollService>();

        return collection;
    }
}