using TPICAP.Matching.Core.Models;

namespace TPICAP.Matching.Core.Abstractions;

public interface IMatchingAlgorithm
{
    IReadOnlyList<MatchedOrder> Match(IReadOnlyList<Order> orders);
}
