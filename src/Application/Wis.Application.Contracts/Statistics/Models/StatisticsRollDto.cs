namespace Application.Contracts.Statistics.Models;

public sealed record StatisticsRollDto(
    int CountAdded,
    int CountRemoved,
    double AverageLength,
    double AverageWeight,
    double MinLength,
    double MaxLength,
    double MinWeight,
    double MaxWeight,
    double TotalWeight,
    TimeSpan MinStorageTime,
    TimeSpan MaxStorageTime,
    DateOnly DayWithMaxCount,
    DateOnly DayWithMinCount,
    DateOnly DayWithMaxWeight,
    DateOnly DayWithMinWeight);