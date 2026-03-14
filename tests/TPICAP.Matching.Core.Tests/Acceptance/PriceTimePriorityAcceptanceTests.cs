using AwesomeAssertions;
using TPICAP.Matching.Core.Algorithms;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Tests.Acceptance;

public class PriceTimePriorityAcceptanceTests
{
    [Fact]
    public void Match_WhenUsingPriceTimePriority_ReturnsExpectedOrderBookState()
    {
        // Arrange
        var algorithm = new PriceTimePriorityMatchingAlgorithm();
        var orders = new[]
        {
            new Order("A1", OrderDirection.Buy, 100, 4.99m, new TimeOnly(9, 27, 43)),
            new Order("B1", OrderDirection.Buy, 200, 5.00m, new TimeOnly(10, 21, 46)),
            new Order("C1", OrderDirection.Buy, 150, 5.00m, new TimeOnly(10, 26, 18)),
            new Order("D1", OrderDirection.Sell, 150, 5.00m, new TimeOnly(10, 32, 41)),
            new Order("E1", OrderDirection.Sell, 100, 5.00m, new TimeOnly(10, 33, 07))
        };

        // Act
        var orderBook = algorithm.Match(orders);

        // Assert
        orderBook.Should().HaveCount(5);

        var a1 = orderBook[0];
        a1.OrderId.Should().Be("A1");
        a1.Direction.Should().Be(OrderDirection.Buy);
        a1.Volume.Should().Be(100);
        a1.Notional.Should().Be(499.00m);
        a1.MatchState.Should().Be(MatchState.NoMatch);
        a1.Matches.Should().BeEmpty();

        var b1 = orderBook[1];
        b1.OrderId.Should().Be("B1");
        b1.Direction.Should().Be(OrderDirection.Buy);
        b1.Volume.Should().Be(200);
        b1.Notional.Should().Be(1000.00m);
        b1.MatchState.Should().Be(MatchState.FullMatch);
        b1.Matches.Should().HaveCount(2);
        b1.Matches[0].OrderId.Should().Be("D1");
        b1.Matches[0].Volume.Should().Be(150);
        b1.Matches[0].Notional.Should().Be(750.00m);
        b1.Matches[1].OrderId.Should().Be("E1");
        b1.Matches[1].Volume.Should().Be(50);
        b1.Matches[1].Notional.Should().Be(250.00m);

        var c1 = orderBook[2];
        c1.OrderId.Should().Be("C1");
        c1.Direction.Should().Be(OrderDirection.Buy);
        c1.Volume.Should().Be(150);
        c1.Notional.Should().Be(750.00m);
        c1.MatchState.Should().Be(MatchState.PartialMatch);
        c1.Matches.Should().HaveCount(1);
        c1.Matches[0].OrderId.Should().Be("E1");
        c1.Matches[0].Volume.Should().Be(50);
        c1.Matches[0].Notional.Should().Be(250.00m);

        var d1 = orderBook[3];
        d1.OrderId.Should().Be("D1");
        d1.Direction.Should().Be(OrderDirection.Sell);
        d1.Volume.Should().Be(150);
        d1.Notional.Should().Be(750.00m);
        d1.MatchState.Should().Be(MatchState.FullMatch);
        d1.Matches.Should().HaveCount(1);
        d1.Matches[0].OrderId.Should().Be("B1");
        d1.Matches[0].Volume.Should().Be(150);
        d1.Matches[0].Notional.Should().Be(750.00m);

        var e1 = orderBook[4];
        e1.OrderId.Should().Be("E1");
        e1.Direction.Should().Be(OrderDirection.Sell);
        e1.Volume.Should().Be(100);
        e1.Notional.Should().Be(500.00m);
        e1.MatchState.Should().Be(MatchState.FullMatch);
        e1.Matches.Should().HaveCount(2);
        e1.Matches[0].OrderId.Should().Be("B1");
        e1.Matches[0].Volume.Should().Be(50);
        e1.Matches[0].Notional.Should().Be(250.00m);
        e1.Matches[1].OrderId.Should().Be("C1");
        e1.Matches[1].Volume.Should().Be(50);
        e1.Matches[1].Notional.Should().Be(250.00m);
    }
}
