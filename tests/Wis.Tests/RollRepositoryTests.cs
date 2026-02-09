using Application.Abstractions.Persistence.Queries;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Wis.Tests;

public class RollRepositoryTests
{
    private WarehouseDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new WarehouseDbContext(options);
    }

    [Fact]
    public async Task Add_And_GetById_ShouldWorkCorrectly()
    {
        // Arrange
        using var context = CreateDbContext();
        var repository = new RollsRepository(context);
        
        var originalRoll = new Roll(Guid.NewGuid(), new Length(15), new Weight(250), DateTimeOffset.Now);
        
        var query = RollsQuery.Build(b => b.WithId(originalRoll.Id));

        // Act
        await repository.AddAsync(originalRoll);
        await context.SaveChangesAsync();

        var rolls = await repository.QueryAsync(query);
        var retrievedRoll = rolls.SingleOrDefault();

        // Assert
        retrievedRoll.Should().NotBeNull();
        retrievedRoll.Id.Should().Be(originalRoll.Id);
        retrievedRoll.Weight.Value.Should().Be(250);
        retrievedRoll.Length.Value.Should().Be(15);
    }

    [Fact]
    public async Task QueryAsync_WithWeightFilter_ShouldReturnOnlyMatchingRolls()
    {
        // Arrange
        using var context = CreateDbContext();
        var repository = new RollsRepository(context);

        var roll1 = new Roll(Guid.NewGuid(), new Length(10), new Weight(100), DateTimeOffset.Now);
        var roll2 = new Roll(Guid.NewGuid(), new Length(10), new Weight(200), DateTimeOffset.Now);
        var roll3 = new Roll(Guid.NewGuid(), new Length(10), new Weight(300), DateTimeOffset.Now);

        await repository.AddAsync(roll1);
        await repository.AddAsync(roll2);
        await repository.AddAsync(roll3);
        await context.SaveChangesAsync();

        var filter = RollsQuery.Build(builder => builder.WithWeight(200.0));

        // Act
        var result = await repository.QueryAsync(filter);

        // Assert
        var resList = result.ToList();
        resList.Should().HaveCount(1);
        resList.First().Weight.Value.Should().Be(200);
    }

    [Fact]
    public async Task Update_ShouldChangeEntityInDatabase()
    {
        // Arrange
        using var context = CreateDbContext();
        var repository = new RollsRepository(context);
        
        var roll = new Roll(Guid.NewGuid(), new Length(10), new Weight(100), DateTimeOffset.Now);
        await repository.AddAsync(roll);
        await context.SaveChangesAsync();

        // Act
        roll.Remove();
        await repository.Update(roll);
        await context.SaveChangesAsync();

        // Assert
        var entityInDb = await context.Rolls.FirstAsync();
        entityInDb.RemovedDate.Should().NotBeNull();
    }
}