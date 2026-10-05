# InterviewApp

This is a starter .NET Core console application designed for interview purposes. The goal is to evaluate your understanding of:

- .NET Core Console Application structure
- Dependency Injection (DI)
- Configuration via `appsettings.json`
- MediatR for decoupled request handling
- Debugging
- Clean code and extensibility principles

---

## 🧠 Tasks

Please complete the following tasks. You may use any libraries or patterns you are comfortable with, but aim for clarity, maintainability, and testability.

### 1. Extend the Greeting Service

- Modify `GreetingService` to support multiple languages (e.g., English, Afrikaans, Zulu).
- Use the `Language` property from `appsettings.json` to determine which greeting to display.
- Add a fallback if the language is not supported.

### 2. Add Logging

- Inject `ILogger<GreetingService>` and log:
    - When the service starts
    - What message is displayed
    - Any errors or unsupported languages
  
### 3. Validate Configuration

- Ensure that the `Message` and `Language` values in `appsettings.json` are not null or empty.
- If validation fails, log an error and exit gracefully.

### 4. Add a Time-Based Greeting Option

- Add a new service `ITimeGreetingService` that returns a greeting based on the current time (e.g., "Good morning", "Good afternoon").
- Inject this service into `GreetingService` and combine it with the configured message.

### 5. Unit Tests (Optional)

- Write unit tests for `GreetingService` using a testing framework of your choice (e.g., xUnit, NUnit).
- Mock dependencies where appropriate.

---

## 🧭 Advanced Tasks: MediatR Integration

These tasks are designed to assess your familiarity with the MediatR library and how to use it to decouple application logic.

### 6. Integrate MediatR

- Add the MediatR NuGet package to the project.
- Register MediatR in the DI container.

### 7. Create a Greeting Request

- Define a `GreetUserCommand` (or `Query`) that encapsulates the greeting logic.
- Implement a `GreetUserHandler` that handles the command and returns the greeting message.
- Replace the direct call to `GreetingService.Run()` with a MediatR `Send()` call.

### 8. Add a Time-Based Greeting Request
- Create a separate `GetTimeGreetingQuery` and handler.
- Combine the result with the configured greeting message using MediatR.

---

## 🧰 Suggested NuGet Packages

```bash
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

---

## 🛠️ Setup Instructions

1. Ensure you have .NET 8 SDK or later installed
2. Run the application:

Note: You may use any IDE of the following: Visual Studio, Visual Studio Code, or Jetbrains Rider

```bash
dotnet run
```

---

## ✅ Implementation Notes - Sean Mthembu

### Tasks Completed
All 8 tasks have been completed, including the optional unit tests (Task 5).

### Architecture Decisions

**MediatR flow**
`Program.cs` sends a `GreetUserCommand` via MediatR. The handler sends a `GetTimeGreetingQuery` to a second handler which returns the time-based greeting. The two results are combined into the final message. Neither handler knows about the other directly — all communication goes through MediatR.

**Configuration validation**
`GreetingOptions` uses `[Required]` data annotations combined with `.ValidateDataAnnotations().ValidateOnStart()`. Validation fires during `host.StartAsync()` — before any handler runs. If `Message` or `Language` is missing, `OptionsValidationException` is caught in `Program.cs`, logged via `ILogger`, and the app exits with code `1`. This is preferred over manual null checks inside business logic.

**App lifecycle**
`Program.cs` uses `StartAsync()` → `mediator.Send()` → `StopAsync()` so the app exits cleanly after doing its work. `RunAsync()` was deliberately avoided as it is designed for long-running services, not one-shot console apps.

**Service registration**
All DI registrations are in a `ServiceCollectionExtensions` extension method, keeping `Program.cs` clean. `TimeProvider.System` is registered as a singleton so `TimeGreetingService` can be tested with a mocked `TimeProvider`.

**Language support**
All 11 official South African languages are supported. An unsupported language logs a warning naming both the requested language and the fallback actually used, then continues with English.

**Environment-specific config**
`appsettings.Development.json` overrides the base config in Development mode. `CreateDefaultBuilder` loads config in the correct order: `appsettings.json` → `appsettings.{Environment}.json` → environment variables → command-line args. No custom `AddJsonFile` call is made, as that would break the provider order.

**MediatR licence**
MediatR v14 is used, which is under a commercial licence (LuckyPenny Software). For production use a licence is required. For open-source or evaluation use, pinning to MediatR v12 avoids this requirement. This project uses v14 for development purposes.

**Logging**
`GreetingService` logs service start, the full assembled message, and any fallback events. The brief explicitly asks for the displayed message to be logged, so it is included. In a production system containing personal data, this log line would be reviewed as part of a data-handling assessment.

### Running the Tests

```bash
dotnet test
```

Tests cover:
- All 11 official South African languages (parametrised `[Theory]`)
- Case-insensitive language lookup
- Unsupported language fallback to English
- Null and empty `Message` and `Language` config values
- Time boundary cases (11:59 → morning, 12:00 → afternoon, 17:59 → afternoon, 18:00 → evening)
- `GreetUserCommandHandler` logic with mocked `IGreetingService` and `IMediator`
- `TimeGreetingService` via injected `TimeProvider` — tests are not time-dependent


