using TPICAP.Matching.Core.Enums;

namespace TPICAP.Matching.Core.Models;

public sealed record MatchedOrder(
    string CompanyId,
    string OrderId,
    OrderDirection Direction,
    int Volume,
    decimal Notional,
    MatchState MatchState,
    IReadOnlyList<MatchEntry> Matches);
