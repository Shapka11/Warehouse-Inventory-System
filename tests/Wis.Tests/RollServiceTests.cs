using Application.Abstractions.Persistence;
using Application.Abstractions.Persistence.Queries;
using Application.Abstractions.Persistence.Repositories;
using Application.Contracts.Rolls.Operations;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Wis.Tests;

public class RollServiceTests
{
    private readonly IPersistenceContext _context;
    private readonly IRollsRepository _repo;
    private readonly RollService _service;

    public RollServiceTests()
    {
        _context = Substitute.For<IPersistenceContext>();
        _repo = Substitute.For<IRollsRepository>();

        _context.RollsRepository.Returns(_repo);

        _service = new RollService(_context);
    }

    [Fact]
    public async Task AddRollAsync_ShouldCreate_And_SaveThroughContext()
    {
        // Arrange
        var request = new AddRoll.Request(Length: 10.0, Weight: 20.0);

        // Act
        var response = await _service.AddRollAsync(request);

        // Assert
        response.Should().BeOfType<AddRoll.Response>();

        await _repo.Received(1).AddAsync(Arg.Any<Roll>(), Arg.Any<CancellationToken>());
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveRollAsync_WhenRollExists_ShouldUpdate_And_Save()
    {
        // Arrange
        var id = Guid.NewGuid();
        var roll = new Roll(id, new Length(10), new Weight(100), DateTimeOffset.Now);

        RollsQuery query = RollsQuery.Build(builder => builder.WithId(id));
        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>()).Returns([roll]);

        // Act
        var response = await _service.RemoveRollAsync(new RemoveRoll.Request(id));

        // Assert
        response.Should().BeOfType<RemoveRoll.Response.Success>();
        roll.RemovedDate.Should().NotBeNull();

        await _repo.Received(1).Update(roll, Arg.Any<CancellationToken>());
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveRollAsync_WhenRollNotFound_ShouldReturnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repo.QueryAsync(Arg.Any<RollsQuery>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var response = await _service.RemoveRollAsync(new RemoveRoll.Request(id));

        // Assert
        response.Should().BeOfType<RemoveRoll.Response.Failure>();
        await _context.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}