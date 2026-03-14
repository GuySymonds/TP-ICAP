using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Algorithms;

internal static class MatchingAlgorithmSupport
{
    public static MatchState GetMatchState(int originalVolume, IReadOnlyList<MatchEntry> matches)
    {
        var matchedVolume = matches.Sum(match => match.Volume);

        if (matchedVolume == 0)
        {
            return MatchState.NoMatch;
        }

        return matchedVolume == originalVolume
            ? MatchState.FullMatch
            : MatchState.PartialMatch;
    }

    public static IReadOnlyList<MatchedOrder> BuildMatchedOrders(
        IReadOnlyList<Order> orders,
        IReadOnlyDictionary<string, List<MatchEntry>> matches)
    {
        return orders
            .Select(order => new MatchedOrder(
                order.CompanyId,
                order.OrderId,
                order.Direction,
                order.Volume,
                order.Notional,
                GetMatchState(order.Volume, matches[order.OrderId]),
                matches[order.OrderId]))
            .ToArray();
    }
}
