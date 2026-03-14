# TP ICAP Matching Challenge

![Coverage](https://raw.githubusercontent.com/GuySymonds/TP-ICAP/main/coveragereport/badge_linecoverage.svg)

## Intent

This repository contains a small, production-minded C# solution for the TP ICAP back-end technical challenge.

The goal is to implement the required order matching behaviour, provide automated tests, include a console application with hard-coded input, and answer the written technical questions in `docs/answers.md`. The handout explicitly asks for a console application with hard-coded input, automated tests, and no additional input mechanisms such as CSV, JSON, text parsers, or REST endpoints. 

## Scope

This solution implements:
- price-time-priority matching
- pro-rata matching
- automated tests using xUnit v3 and Awesome Assertions
- a console demo using hard-coded scenarios
- supporting documentation

This solution intentionally does not implement:
- file parsing
- APIs
- persistence
- external configuration
- unnecessary infrastructure

## Structure

- `src/`
  - `TPICAP.Matching.Core/`
  - `TPICAP.Matching.Console/`
- `tests/`
  - `TPICAP.Matching.Core.Tests/`
- `docs/`
  - `answers.md`

## Approach

The implementation is being built using strict TDD:
- Red
- Green
- Refactor

Work starts from the acceptance tests based on the two explicit challenge scenarios, then grows only as needed to support those tests. The handout provides two specific user stories with acceptance criteria for price-time-priority and pro-rata matching.  

## Technology

- C#
- .NET 10
- xUnit v3
- Awesome Assertions
- Spectre.Console

## Running the Solution

### Run the tests

```bash
dotnet test
```

### Run tests with code coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

Coverage files are written under `tests/TPICAP.Matching.Core.Tests/TestResults/`.

### Run the console demo

```bash
dotnet run --project src/TPICAP.Matching.Console
```

## Design Notes

The solution is intentionally small and focused on reviewer readability.

Key choices:

* one `Core` project for business logic
* one `Console` project for demonstration
* one `Tests` project for automated tests
* warnings treated as errors across the solution
* simple abstractions only where behaviour genuinely varies

## Output Model

The returned order book state is aligned to the handout output requirements:

* `CompanyId`
* `OrderId`
* `Direction`
* `Volume`
* `Notional`
* `MatchState`
* `Matches`
  * `OrderId`
  * `Notional`
  * `Volume`

## Assumptions

### Volume meaning in the returned order book

The handout requires the returned order book state to include `Volume`, but does not explicitly define whether that means original volume or remaining unmatched volume. For this solution, `Volume` is treated as the original order volume. Match outcomes are represented through `MatchState` and the `Matches` collection. No additional output fields such as remaining or open volume have been introduced.

### Permissive open-source packages

The handout states that frameworks, packages, and libraries may be used provided they do not require a license. This solution interprets that as allowing permissively licensed open-source packages, such as MIT-licensed packages, provided no paid, commercial, or separately purchased license is required to build, run, or review the solution.

## Challenge Notes

The handout also asks for answers to the written technical questions in `answers.md`. Those are captured in:

* `docs/answers.md`
