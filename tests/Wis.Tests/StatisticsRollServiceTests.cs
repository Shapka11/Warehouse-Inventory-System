using Application.Abstractions.Persistence;
using Application.Abstractions.Persistence.Queries;
using Application.Abstractions.Persistence.Repositories;
using Application.Contracts.Statistics.Operations;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Wis.Tests;

public class StatisticsRollServiceTests
{
    private readonly IPersistenceContext _context;
    private readonly IRollsRepository _repo;
    private readonly StatisticsRollService _service;

    public StatisticsRollServiceTests()
    {
        _context = Substitute.For<IPersistenceContext>();
        _repo = Substitute.For<IRollsRepository>();
        _context.RollsRepository.Returns(_repo);

        _service = new StatisticsRollService(_context);
    }

    [Fact]
    public async Task GetStatistics_ShouldCalculateCorrectPhysicalMetrics()
    {
        // Arrange
        var from = DateTimeOffset.UtcNow.AddDays(-10);
        var to = DateTimeOffset.UtcNow;

        var rolls = new List<Roll>
        {
            new(Guid.NewGuid(), new Length(10), new Weight(100), from),
            new(Guid.NewGuid(), new Length(20), new Weight(200), from),
            new(Guid.NewGuid(), new Length(30), new Weight(300), from)
        };

        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>()).Returns(rolls);

        // Act
        var response = await _service.GetStatisticsAsync(new GetStatisticsRoll.Request(from, to));

        // Assert
        var stats = response.Should().BeOfType<GetStatisticsRoll.Response.Success>().Which.StatisticsRoll;

        stats.AverageWeight.Should().Be(200);
        stats.AverageLength.Should().Be(20);
        stats.MaxWeight.Should().Be(300);
        stats.MinLength.Should().Be(10);
        stats.TotalWeight.Should().Be(600);
    }

    [Fact]
    public async Task GetStatistics_ShouldCalculateCorrectStorageIntervals()
    {
        // Arrange
        var start = DateTimeOffset.UtcNow.AddDays(-30);

        var rollShort = new Roll(Guid.NewGuid(), new Length(10), new Weight(100),
            start, start.AddDays(2));

        var rollLong = new Roll(Guid.NewGuid(), new Length(10), new Weight(100),
            start, start.AddDays(10));

        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>())
            .Returns([rollShort, rollLong]);

        // Act
        var response = await _service.GetStatisticsAsync(new GetStatisticsRoll.Request(start, DateTimeOffset.UtcNow));

        // Assert
        var stats = response.Should().BeOfType<GetStatisticsRoll.Response.Success>().Which.StatisticsRoll;
        stats.MinStorageTime.Should().Be(TimeSpan.FromDays(2));
        stats.MaxStorageTime.Should().Be(TimeSpan.FromDays(10));
    }

    [Fact]
    public async Task GetStatistics_ShouldIdentifyCorrectPeakDays()
    {
        // Arrange
        var day1 = new DateTimeOffset(2023, 10, 1, 0, 0, 0, TimeSpan.Zero);
        var day2 = day1.AddDays(1);
        var day3 = day1.AddDays(2);

        var rolls = new List<Roll>
        {
            // День 1: 2 рулона, общий вес 200
            new(Guid.NewGuid(), new Length(10), new Weight(100), day1, day3),
            new(Guid.NewGuid(), new Length(10), new Weight(100), day1, day3),
            new(Guid.NewGuid(), new Length(10), new Weight(500), day2, day3)
        };

        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>()).Returns(rolls);

        // Act
        var response = await _service.GetStatisticsAsync(new GetStatisticsRoll.Request(day1, day2));

        // Assert
        var stats = response.Should().BeOfType<GetStatisticsRoll.Response.Success>().Which.StatisticsRoll;
        stats.DayWithMaxCount.Should().Be(DateOnly.FromDateTime(day2.DateTime));
        stats.DayWithMaxWeight.Should().Be(DateOnly.FromDateTime(day2.DateTime));
        stats.DayWithMinCount.Should().Be(DateOnly.FromDateTime(day1.DateTime));
    }

    [Fact]
    public async Task GetStatistics_WithNoData_ShouldReturnZeros()
    {
        // Arrange
        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>())
            .Returns(new List<Roll>());

        // Act
        var response = await _service.GetStatisticsAsync(
            new GetStatisticsRoll.Request(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow));

        // Assert
        var stats = response.Should().BeOfType<GetStatisticsRoll.Response.Success>().Which.StatisticsRoll;
        stats.CountAdded.Should().Be(0);
        stats.TotalWeight.Should().Be(0);
        stats.MaxStorageTime.Should().Be(TimeSpan.Zero);
    }
}