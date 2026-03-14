# Testing Instructions

- Use xUnit v3 and Awesome Assertions
- Follow strict Red, Green, Refactor
- Start from acceptance tests before adding lower-level tests
- Use `MethodName_Scenario_ExpectedOutcome`
- Add Arrange, Act, Assert comments inside each test
- Prefer small, behaviour-focused tests
- Avoid asserting implementation details
- Avoid giant equivalence assertions for full result graphs
- In acceptance tests, assert order-by-order for readability

## First Tests To Create

Create these two acceptance test files first:
- `tests/TPICAP.Matching.Core.Tests/Acceptance/PriceTimePriorityAcceptanceTests.cs`
- `tests/TPICAP.Matching.Core.Tests/Acceptance/ProRataAcceptanceTests.cs`

Create these two tests first:
- `Match_WhenUsingPriceTimePriority_ReturnsExpectedOrderBookState`
- `Match_WhenUsingProRata_ReturnsExpectedOrderBookState`

Rules for these tests:
- use the exact sample data from the handout
- assert the full returned order book state
- assert results order-by-order for readability
- do not write production code before these tests exist

## Exact First Acceptance Scenario: Price-Time-Priority

Use this exact order book:
- A1 Buy 100 @ 4.99 at 09:27:43
- B1 Buy 200 @ 5.00 at 10:21:46
- C1 Buy 150 @ 5.00 at 10:26:18
- D1 Sell 150 @ 5.00 at 10:32:41
- E1 Sell 100 @ 5.00 at 10:33:07

Assert this exact outcome:
- A1 = `NoMatch`
- B1 = `FullMatch`
  - D1 = 150
  - E1 = 50
- C1 = `PartialMatch`
  - E1 = 50
- D1 = `FullMatch`
  - B1 = 150
- E1 = `FullMatch`
  - B1 = 50
  - C1 = 50

## Exact First Acceptance Scenario: Pro-Rata

Use this exact order book:
- A1 Buy 50 @ 5.00 at 09:27:43
- B1 Buy 200 @ 5.00 at 10:21:46
- C1 Sell 200 @ 5.00 at 10:26:18

Assert this exact outcome:
- A1 = `PartialMatch`
  - C1 = 40
- B1 = `PartialMatch`
  - C1 = 160
- C1 = `FullMatch`
  - A1 = 40
  - B1 = 160
