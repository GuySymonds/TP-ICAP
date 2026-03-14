using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Algorithms;

internal static class MatchingAlgorithmSupport
{
    public static void ValidateInput(IReadOnlyList<Order> orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        var seenOrderIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var order in orders)
        {
            if (!seenOrderIds.Add(order.OrderId))
            {
                throw new ArgumentException($"Duplicate OrderId detected: '{order.OrderId}'.", nameof(orders));
            }
        }
    }

    public static HashSet<string> GetInvalidOrderIds(IReadOnlyList<Order> orders)
    {
        return orders
            .Where(IsInvalidOrder)
            .Select(order => order.OrderId)
            .ToHashSet(StringComparer.Ordinal);
    }

    private static bool IsInvalidOrder(Order order)
    {
        return string.IsNullOrWhiteSpace(order.CompanyId)
            || string.IsNullOrWhiteSpace(order.OrderId)
            || order.Volume <= 0
            || order.Price <= 0m;
    }

    public static MatchState GetMatchState(int originalVolume, IReadOnlyList<MatchEntry> matches, bool isInvalidOrder)
    {
        if (isInvalidOrder)
        {
            return MatchState.InvalidOrder;
        }

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
        IReadOnlyDictionary<string, List<MatchEntry>> matches,
        IReadOnlySet<string> invalidOrderIds)
    {
        return orders
            .Select(order => new MatchedOrder(
                order.CompanyId,
                order.OrderId,
                order.Direction,
                order.Volume,
                order.Notional,
                GetMatchState(order.Volume, matches[order.OrderId], invalidOrderIds.Contains(order.OrderId)),
                matches[order.OrderId]))
            .ToArray();
    }
}
