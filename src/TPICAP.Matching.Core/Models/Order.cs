using TPICAP.Matching.Core.Enums;

namespace TPICAP.Matching.Core.Models;

public sealed record Order(string OrderId, OrderDirection Direction, int Volume, decimal Price, TimeOnly Timestamp)
{
    public decimal Notional => Volume * Price;
}
