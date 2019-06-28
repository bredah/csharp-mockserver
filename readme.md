# .NET Core C# API for interacting with Mock-Server

## Setup

Running the Mock-Server into local machine or server.

Docker CLI example:

```bash
docker run -p 1080:1080 jamesdbloom/mockserver
```

## Running

Look the examples below:

- [Create a Expectation - Get](./MockServer.Tests/MockTest.cs#L45)
- [Create a Expectation - Post](./MockServer.Tests/MockTest.cs#L92)
- [Create a Expectation - Put](./MockServer.Tests/MockTest.cs#L122)
- [Create a Expectation - Delete](./MockServer.Tests/MockTest.cs#L152)
- [Create a Expectation - Unique Session](./MockServer.Tests/MockTest.cs#L66)
- [Remove a Expectation](./MockServer.Tests/MockTest.cs#L170)
