using TPICAP.Matching.Core.Abstractions;
using TPICAP.Matching.Core.Enums;
using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Algorithms;

public sealed class PriceTimePriorityMatchingAlgorithm : IMatchingAlgorithm
{
    public IReadOnlyList<MatchedOrder> Match(IReadOnlyList<Order> orders)
    {
        MatchingAlgorithmSupport.ValidateInput(orders);

        var invalidOrderIds = MatchingAlgorithmSupport.GetInvalidOrderIds(orders);
        var validOrders = orders.Where(order => !invalidOrderIds.Contains(order.OrderId)).ToArray();

        var remainingVolumes = validOrders.ToDictionary(order => order.OrderId, order => order.Volume, StringComparer.Ordinal);
        var matches = orders.ToDictionary(order => order.OrderId, _ => new List<MatchEntry>(), StringComparer.Ordinal);
        var buyOrders = validOrders
            .Where(order => order.Direction == OrderDirection.Buy)
            .OrderByDescending(order => order.Notional)
            .ThenBy(order => order.Timestamp)
            .ToArray();

        foreach (var sellOrder in validOrders.Where(order => order.Direction == OrderDirection.Sell))
        {
            var remainingSellVolume = remainingVolumes[sellOrder.OrderId];

            foreach (var buyOrder in buyOrders)
            {
                if (remainingSellVolume == 0)
                {
                    break;
                }

                if (buyOrder.Price != sellOrder.Price)
                {
                    continue;
                }

                var remainingBuyVolume = remainingVolumes[buyOrder.OrderId];
                if (remainingBuyVolume == 0)
                {
                    continue;
                }

                var matchedVolume = Math.Min(remainingBuyVolume, remainingSellVolume);
                var matchedNotional = matchedVolume * sellOrder.Price;

                matches[buyOrder.OrderId].Add(new MatchEntry(sellOrder.OrderId, matchedNotional, matchedVolume));
                matches[sellOrder.OrderId].Add(new MatchEntry(buyOrder.OrderId, matchedNotional, matchedVolume));

                remainingVolumes[buyOrder.OrderId] = remainingBuyVolume - matchedVolume;
                remainingSellVolume -= matchedVolume;
            }

            remainingVolumes[sellOrder.OrderId] = remainingSellVolume;
        }

        return MatchingAlgorithmSupport.BuildMatchedOrders(orders, matches, invalidOrderIds);
    }
}
