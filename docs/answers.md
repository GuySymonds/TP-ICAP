# answers

## 1. How much time did you spend on the engineering task?

I spent approximately **3.5 hours** on the engineering task.

## 2. What would you add to your solution if you’d had more time?

- Stronger validation for malformed orders (missing IDs, non-positive volume, invalid price).
- Explicit handling for invalid orders using `MatchState.InvalidOrder` where appropriate.
- Additional tests for edge cases and regression scenarios.
- Performance benchmarks for larger order books.
- More operational diagnostics (structured logging and metrics around matching decisions).

## 3. What do you think is the most useful feature added to the latest version of C#?

One of the most useful recent additions is **primary constructors**, because they reduce boilerplate for immutable, data-focused types while keeping intent clear.

### 3a. Include a code snippet that shows how you've used it.

This solution uses primary-constructor style records for core domain models:

```csharp
public sealed record Order(
    string CompanyId,
    string OrderId,
    OrderDirection Direction,
    int Volume,
    decimal Price,
    TimeOnly Timestamp)
{
    public decimal Notional => Volume * Price;
}
```

## 4. How would you track down a performance issue in production?

I would use a measurement-first workflow:

1. Confirm the symptom using production telemetry (latency, throughput, CPU, memory).
2. Isolate the hot path by correlating traces and logs with slow operations.
3. Reproduce in a controlled environment with realistic data.
4. Profile CPU and allocations to identify dominant methods and allocation sources.
5. Apply the smallest safe optimization.
6. Re-measure and compare against baseline before and after deployment.

### 4a. Have you ever had to do this?

Yes. I have previously investigated high-latency request paths by correlating tracing data with profiler output, then removing unnecessary allocations and repeated collection scans in the hot path. The fix reduced p95 latency and stabilized CPU utilization under peak load.
