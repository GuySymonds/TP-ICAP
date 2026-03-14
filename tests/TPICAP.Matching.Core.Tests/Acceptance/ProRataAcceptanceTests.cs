using FluentAssertions;
using Xunit;
using TPICAP.Matching.Core.Algorithms;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Tests.Acceptance;

public class ProRataAcceptanceTests
{
    [Fact]
    public void Match_WhenUsingProRata_ReturnsExpectedOrderBookState()
    {
        // Arrange
        var algorithm = new ProRataMatchingAlgorithm();
        var orders = new[]
        {
            new Order("A1", OrderDirection.Buy, 50, 5.00m, new TimeOnly(9, 27, 43)),
            new Order("B1", OrderDirection.Buy, 200, 5.00m, new TimeOnly(10, 21, 46)),
            new Order("C1", OrderDirection.Sell, 200, 5.00m, new TimeOnly(10, 26, 18))
        };

        // Act
        var orderBook = algorithm.Match(orders);

        // Assert
        orderBook.Should().HaveCount(3);

        var a1 = orderBook[0];
        a1.OrderId.Should().Be("A1");
        a1.Direction.Should().Be(OrderDirection.Buy);
        a1.Volume.Should().Be(50);
        a1.Notional.Should().Be(250.00m);
        a1.MatchState.Should().Be(MatchState.PartialMatch);
        a1.Matches.Should().HaveCount(1);
        a1.Matches[0].OrderId.Should().Be("C1");
        a1.Matches[0].Volume.Should().Be(40);
        a1.Matches[0].Notional.Should().Be(200.00m);

        var b1 = orderBook[1];
        b1.OrderId.Should().Be("B1");
        b1.Direction.Should().Be(OrderDirection.Buy);
        b1.Volume.Should().Be(200);
        b1.Notional.Should().Be(1000.00m);
        b1.MatchState.Should().Be(MatchState.PartialMatch);
        b1.Matches.Should().HaveCount(1);
        b1.Matches[0].OrderId.Should().Be("C1");
        b1.Matches[0].Volume.Should().Be(160);
        b1.Matches[0].Notional.Should().Be(800.00m);

        var c1 = orderBook[2];
        c1.OrderId.Should().Be("C1");
        c1.Direction.Should().Be(OrderDirection.Sell);
        c1.Volume.Should().Be(200);
        c1.Notional.Should().Be(1000.00m);
        c1.MatchState.Should().Be(MatchState.FullMatch);
        c1.Matches.Should().HaveCount(2);
        c1.Matches[0].OrderId.Should().Be("A1");
        c1.Matches[0].Volume.Should().Be(40);
        c1.Matches[0].Notional.Should().Be(200.00m);
        c1.Matches[1].OrderId.Should().Be("B1");
        c1.Matches[1].Volume.Should().Be(160);
        c1.Matches[1].Notional.Should().Be(800.00m);
    }
}
