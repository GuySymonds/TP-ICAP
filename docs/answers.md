# Answers

## How to make this solution production ready

- Add stronger input validation around orders, timestamps, prices, and volumes.
- Separate the current in-memory demo concerns from application boundaries such as request handling and persistence.
- Expand automated coverage beyond the handout stories with edge cases, invalid input cases, and regression tests.
- Add structured logging, metrics, and health checks.
- Define versioned contracts for inbound orders and outbound order book results.
- Add persistence or event capture if auditability and replay are required.
- Harden error handling around external boundaries while keeping business flow explicit.
- Add CI checks for restore, build, test, and formatting.

## What would change under higher scale or throughput

- Move from a simple list-based in-memory approach to data structures designed for fast book updates and price-level lookup.
- Partition processing by instrument or book so matching can run independently where safe.
- Minimise allocations in the hot path and benchmark before optimising further.
- Introduce asynchronous ingestion around the matching engine, while keeping matching itself deterministic.
- Add back-pressure and queueing strategies for bursts of inbound traffic.
- Capture latency and throughput metrics to guide optimisation rather than guessing.
- If persistence is needed, prefer append-only event capture and replay over synchronous write-heavy flows in the hot path.

## Additional concerns in a real trading or matching environment

- Determinism: the same input sequence must always produce the same output.
- Auditability: retain enough information to reconstruct decisions and replay the book.
- Time handling: use precise timestamps and a clear policy for ordering ties.
- Concurrency control: prevent races that could alter book state.
- Resilience: plan for restart, replay, and partial failure handling.
- Observability: log key events and publish operational metrics.
- Security and access control around order submission and operational tooling.
- Operational support such as alerting, runbooks, and safe deployment practices.
- Clear handling of ambiguous business rules. In this repo, assumptions remain as documented in `README.md`, including treating returned `Volume` as original order volume.

## Scope of the current implementation

- The current repo intentionally implements only the handout scenarios for price-time-priority and pro-rata matching.
- The console app uses hard-coded input only.
- No APIs, file parsing, persistence, external configuration, or dependency injection have been added.
- The current design is intentionally small and reviewer-friendly rather than production-complete.
