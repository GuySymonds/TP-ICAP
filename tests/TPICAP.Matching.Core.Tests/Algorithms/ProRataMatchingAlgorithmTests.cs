using FluentAssertions;
using TPICAP.Matching.Core.Algorithms;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;
using Xunit;

namespace TPICAP.Matching.Core.Tests.Algorithms;

public class ProRataMatchingAlgorithmTests
{
    [Fact]
    public void Match_WhenBuyVolumesDiffer_AllocatesProportionally()
    {
        // Arrange
        var algorithm = new ProRataMatchingAlgorithm();
        var orders = new[]
        {
            new Order("A1", OrderDirection.Buy, 50, 5.00m, new TimeOnly(9, 0, 0)),
            new Order("B1", OrderDirection.Buy, 200, 5.00m, new TimeOnly(10, 0, 0)),
            new Order("S1", OrderDirection.Sell, 200, 5.00m, new TimeOnly(11, 0, 0))
        };

        // Act
        var orderBook = algorithm.Match(orders);

        // Assert
        orderBook.Should().HaveCount(3);

        var a1 = orderBook[0];
        a1.OrderId.Should().Be("A1");
        a1.MatchState.Should().Be(MatchState.PartialMatch);
        a1.Matches.Should().HaveCount(1);
        a1.Matches[0].OrderId.Should().Be("S1");
        a1.Matches[0].Volume.Should().Be(40);
        a1.Matches[0].Notional.Should().Be(200.00m);

        var b1 = orderBook[1];
        b1.OrderId.Should().Be("B1");
        b1.MatchState.Should().Be(MatchState.PartialMatch);
        b1.Matches.Should().HaveCount(1);
        b1.Matches[0].OrderId.Should().Be("S1");
        b1.Matches[0].Volume.Should().Be(160);
        b1.Matches[0].Notional.Should().Be(800.00m);

        var s1 = orderBook[2];
        s1.OrderId.Should().Be("S1");
        s1.MatchState.Should().Be(MatchState.FullMatch);
        s1.Matches.Should().HaveCount(2);
        s1.Matches[0].OrderId.Should().Be("A1");
        s1.Matches[0].Volume.Should().Be(40);
        s1.Matches[0].Notional.Should().Be(200.00m);
        s1.Matches[1].OrderId.Should().Be("B1");
        s1.Matches[1].Volume.Should().Be(160);
        s1.Matches[1].Notional.Should().Be(800.00m);
    }
}
