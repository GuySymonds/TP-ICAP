using FluentAssertions;
using TPICAP.Matching.Core.Algorithms;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;
using Xunit;

namespace TPICAP.Matching.Core.Tests.Algorithms;

public class PriceTimePriorityMatchingAlgorithmTests
{
    [Fact]
    public void Match_WhenBuyOrdersHaveEqualNotional_PrioritisesEarlierOrderFirst()
    {
        // Arrange
        var algorithm = new PriceTimePriorityMatchingAlgorithm();
        var orders = new[]
        {
            new Order("A1", OrderDirection.Buy, 100, 5.00m, new TimeOnly(9, 0, 0)),
            new Order("B1", OrderDirection.Buy, 100, 5.00m, new TimeOnly(10, 0, 0)),
            new Order("S1", OrderDirection.Sell, 100, 5.00m, new TimeOnly(11, 0, 0))
        };

        // Act
        var orderBook = algorithm.Match(orders);

        // Assert
        orderBook.Should().HaveCount(3);

        var a1 = orderBook[0];
        a1.OrderId.Should().Be("A1");
        a1.MatchState.Should().Be(MatchState.FullMatch);
        a1.Matches.Should().HaveCount(1);
        a1.Matches[0].OrderId.Should().Be("S1");
        a1.Matches[0].Volume.Should().Be(100);

        var b1 = orderBook[1];
        b1.OrderId.Should().Be("B1");
        b1.MatchState.Should().Be(MatchState.NoMatch);
        b1.Matches.Should().BeEmpty();

        var s1 = orderBook[2];
        s1.OrderId.Should().Be("S1");
        s1.MatchState.Should().Be(MatchState.FullMatch);
        s1.Matches.Should().HaveCount(1);
        s1.Matches[0].OrderId.Should().Be("A1");
        s1.Matches[0].Volume.Should().Be(100);
    }
}
