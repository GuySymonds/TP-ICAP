using TPICAP.Matching.Core.Abstractions;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Algorithms;

public sealed class ProRataMatchingAlgorithm : IMatchingAlgorithm
{
    public IReadOnlyList<MatchedOrder> Match(IReadOnlyList<Order> orders)
    {
        return orders
            .Select(order => new MatchedOrder(
                string.Empty,
                order.OrderId,
                order.Direction,
                order.Volume,
                order.Notional,
                MatchState.NoMatch,
                []))
            .ToArray();
    }
}
