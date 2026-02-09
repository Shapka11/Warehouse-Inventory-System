using Domain.Entities;
using Domain.Entities.ResultTypes;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Wis.Tests;

public class RollTests
{
    [Fact]
    public void CreateRoll_WithValidData_ShouldSucceed()
    {
        // Arrange
        var weight = new Weight(100);
        var length = new Length(10);

        // Act
        var roll = new Roll(Guid.NewGuid(), length, weight, DateTimeOffset.Now);

        // Assert
        roll.Id.Should().NotBeEmpty();
        roll.AddedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        roll.RemovedDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CreateWeight_WithInvalidValue_ShouldThrowException(double value)
    {
        // Act
        Action act = () => new Weight(value);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void CreateLength_WithInvalidValue_ShouldThrowException(double value)
    {
        // Act
        Action act = () => new Length(value);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Remove_ShouldSetRemovedDate()
    {
        // Arrange
        var roll = new Roll(Guid.NewGuid(), new Length(10), new Weight(100), DateTimeOffset.Now);

        // Act
        roll.Remove();

        // Assert
        roll.RemovedDate.Should().NotBeNull();
    }

    [Fact]
    public void Remove_AlreadyRemovedRoll_ShouldFailure()
    {
        // Arrange
        var roll = new Roll(Guid.NewGuid(), new Length(10), new Weight(100), DateTimeOffset.Now);

        // Act
        roll.Remove();
        RollRemoveResult result = roll.Remove();

        // Assert
        Assert.IsType<RollRemoveResult.Failure>(result);
    }
}