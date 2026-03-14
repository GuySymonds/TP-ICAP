using TPICAP.Matching.Core.Abstractions;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Algorithms;

public sealed class ProRataMatchingAlgorithm : IMatchingAlgorithm
{
    public IReadOnlyList<MatchedOrder> Match(IReadOnlyList<Order> orders)
    {
        var remainingVolumes = orders.ToDictionary(order => order.OrderId, order => order.Volume, StringComparer.Ordinal);
        var matches = orders.ToDictionary(order => order.OrderId, _ => new List<MatchEntry>(), StringComparer.Ordinal);

        foreach (var sellOrder in orders.Where(order => order.Direction == OrderDirection.Sell))
        {
            var eligibleBuyOrders = orders
                .Where(order => order.Direction == OrderDirection.Buy && order.Price == sellOrder.Price)
                .ToArray();

            if (eligibleBuyOrders.Length == 0)
            {
                continue;
            }

            var remainingSellVolume = remainingVolumes[sellOrder.OrderId];
            var originalSellVolume = remainingSellVolume;
            var totalEligibleBuyVolume = eligibleBuyOrders.Sum(order => remainingVolumes[order.OrderId]);

            if (remainingSellVolume == 0 || totalEligibleBuyVolume == 0)
            {
                continue;
            }

            for (var index = 0; index < eligibleBuyOrders.Length; index++)
            {
                var buyOrder = eligibleBuyOrders[index];
                var remainingBuyVolume = remainingVolumes[buyOrder.OrderId];
                if (remainingBuyVolume == 0)
                {
                    continue;
                }

                var matchedVolume = index == eligibleBuyOrders.Length - 1
                    ? remainingSellVolume
                    : (int)((decimal)originalSellVolume * remainingBuyVolume / totalEligibleBuyVolume);

                if (matchedVolume == 0)
                {
                    continue;
                }

                var matchedNotional = matchedVolume * sellOrder.Price;

                matches[buyOrder.OrderId].Add(new MatchEntry(sellOrder.OrderId, matchedNotional, matchedVolume));
                matches[sellOrder.OrderId].Add(new MatchEntry(buyOrder.OrderId, matchedNotional, matchedVolume));

                remainingVolumes[buyOrder.OrderId] = remainingBuyVolume - matchedVolume;
                remainingSellVolume -= matchedVolume;
            }

            remainingVolumes[sellOrder.OrderId] = remainingSellVolume;
        }

        return MatchingAlgorithmSupport.BuildMatchedOrders(orders, matches);
    }
}
