# Result Pattern Instructions

Use a simple in-house result flow.

Rules:
- create `Result`
- create `Result<T>`
- support success and failure only
- failure contains a message only
- do not introduce error codes
- do not use a third-party results library
- do not return `null` for expected failure paths
- do not use exceptions for expected business flow

Use results for operations that can fail.
Keep simple data models as plain models.
