# Copilot Instructions

## Purpose
Build a small, clear, production-minded C# solution for the TP ICAP back-end challenge.

The solution must:
- implement the required matching behaviour
- include automated tests
- include a console application with hard-coded input
- include supporting documentation in `docs`
- compile and run without additional work from the reviewer

The handout explicitly asks for:
- a console application with hard-coded input
- no extra input mechanisms such as CSV, JSON, text parsers, or REST endpoints
- automated tests
- an `answers.md` file for the technical questions
- use of C#

## Platform
- Use C# and .NET 10
- Use stable SDK features only
- Enable nullable reference types
- Enable implicit usings
- Use file-scoped namespaces

## Working Approach
Follow strict TDD with Red, Green, Refactor.

For every change:
1. Write a failing test first
2. Implement the smallest change needed to make it pass
3. Refactor only after tests are green

Rules:
- work in very small steps
- do not implement future features early
- do not batch unrelated changes together
- start from acceptance tests
- only add lower-level unit tests when they help drive implementation or protect refactoring

## Solution Structure
Use this structure:

- `src/`
  - `TPICAP.Matching.Core/`
  - `TPICAP.Matching.Console/`
- `tests/`
  - `TPICAP.Matching.Core.Tests/`
- `docs/`
- `README.md`

## Internal Project Structure
Within `TPICAP.Matching.Core`, organise code by folder:

- `Abstractions`
- `Algorithms`
- `Models`
- `Enums`
- `Services`
- `Results`

Within `TPICAP.Matching.Core.Tests`, organise tests by folder:

- `Acceptance`
- `Algorithms`
- `Services`

Mirror source structure where it makes sense.
Do not create vague folders such as `Helpers`, `Utils`, or `Common` unless there is a strong reason.

## Design Rules
Keep the design as simple as possible.

Apply SOLID without overengineering:
- use abstractions where behaviour genuinely varies
- keep classes focused
- keep methods short
- prefer explicit, readable code
- avoid speculative architecture

Avoid:
- unnecessary layers
- premature optimisation
- unnecessary patterns
- service locators
- static mutable state
- clever code

## Result Pattern
Use a simple in-house result pattern.

Rules:
- use `Result` for success/failure
- use `Result<T>` for success/failure with a value
- failures should contain a message only
- do not use a third-party result library
- do not return `null` for expected failure cases
- do not use exceptions for expected control flow

Keep pure data models simple. Do not wrap every model in a result type unless an operation can actually fail.

## Build Quality
The solution must build with zero warnings.

Rules:
- treat warnings as errors in all projects, including tests
- do not suppress warnings to get a clean build
- fix the underlying code instead

## Testing Stack
Use:
- xUnit v3
- Awesome Assertions

## Test Naming
Use:
`MethodName_Scenario_ExpectedOutcome`

Examples:
- `Match_WhenUsingPriceTimePriority_ReturnsExpectedOrderBookState`
- `Match_WhenUsingProRata_ReturnsExpectedOrderBookState`
- `Match_WhenHigherPricedBuyExists_PrioritisesThatOrderFirst`

Test class naming:
- unit tests: `{ClassName}Tests`
- acceptance tests: `{FeatureName}AcceptanceTests`

Examples:
- `PriceTimePriorityMatchingAlgorithmTests`
- `ProRataMatchingAlgorithmTests`
- `PriceTimePriorityAcceptanceTests`

## Test Style
Inside each test, use comments for:
- Arrange
- Act
- Assert

Keep tests focused on observable behaviour.
Do not test private implementation details.

## Acceptance Test Rules
Start with the two handout stories.

Create exactly these files first:
- `tests/TPICAP.Matching.Core.Tests/Acceptance/PriceTimePriorityAcceptanceTests.cs`
- `tests/TPICAP.Matching.Core.Tests/Acceptance/ProRataAcceptanceTests.cs`

Start each file with one primary acceptance test based on the exact handout scenario.

Acceptance tests must:
- use the exact scenario data from the challenge
- validate the full returned order book
- assert results order-by-order for readability
- avoid one giant object graph assertion
- be created before production code

## Public API Guidance
Keep the public API small.

Prefer a simple abstraction for matching algorithms with a shared `Match` contract.
Do not introduce extra services unless the tests force the design there.

The returned order book state should align to the brief, which requires:
- `CompanyId`
- `OrderId`
- `Direction`
- `Volume`
- `Notional`
- `MatchState`
- `Matches` with `OrderId`, `Notional`, and `Volume`

Do not invent extra return fields unless they are clearly needed by the challenge.

## Console App Rules
Use Spectre.Console for presentation only.

The console app should:
- use hard-coded sample input
- run the algorithms
- print readable output for a reviewer

Do not add:
- file parsing
- APIs
- persistence
- configuration systems
- dependency injection unless clearly needed

## Documentation Rules
`README.md` must explain:
- repo intent
- challenge scope
- project structure
- how to run tests
- how to run the console app
- design choices
- assumptions

`docs/` should contain supporting challenge material, including:
- `answers.md`

## Assumptions
Where the brief is ambiguous:
- do not silently invent behaviour
- document the assumption in `README.md`

Include at least these assumptions:
- the returned `Volume` is treated as the original order volume, because the handout requires `Volume` but does not explicitly state whether that means original or remaining volume
- permissive open-source packages are allowed, because the handout allows frameworks, packages, and libraries as long as they do not require a license; this solution interprets that as allowing permissively licensed packages such as MIT, provided no paid or separate license is required

## Quality Bar
Optimise for:
- correctness
- clarity
- maintainability
- reviewer readability

Not for:
- framework showcase
- enterprise ceremony
- unnecessary extensibility
