# Copilot Instructions for Ddth.Utilities

## Build, Test, and Lint

```bash
# Restore, build, and test
dotnet restore
dotnet build --configuration=Release
dotnet test --configuration=Release

# Run a single test by fully-qualified name
dotnet test --configuration=Release --filter "FullyQualifiedName~Ddth.Utilities.Tests.RandomHelper.RandomHelperTest.TestRandomChar"

# Run all tests in a specific test class
dotnet test --configuration=Release --filter "FullyQualifiedName~Ddth.Utilities.Tests.ReflectionHelper.ReflectionDIHelperTest"

# Test with code coverage
dotnet test --configuration=Release --collect="Code Coverage;Format=cobertura" --results-directory=TestResults/
```

## Architecture

This is a .NET 6+ NuGet package (`Ddth.Utilities`) providing static utility classes:

- **`RandomUtils`** — Cryptographically secure random value generation (`int`, `short`, `long`, `char`) using `RandomNumberGenerator`. All other random-dependent utilities build on this class.
- **`RandomPasswordGenerator`** — Password generation using `Microsoft.AspNetCore.Identity.PasswordOptions` for strength requirements. Delegates all randomness to `RandomUtils`.
- **`ReflectionDIHelper`** — Creates instances via reflection with automatic constructor-based dependency injection from `IServiceProvider`, with optional additional service overrides.
- **`Tempus`** (`Ddth.Utilities.Tempus` sub-namespace) — Extension methods on `DateTime` and `DateTimeOffset` for start-of-day, weekday navigation, time-window checks, day-of-week matching, and time-zone conversion. Uses `#if NET6_0` conditionals for API differences across target frameworks.

## Conventions

- All utility classes are `static`. Root-level utilities live in `Ddth.Utilities`; feature groups use sub-namespaces with their own subdirectory (e.g., `Tempus/` → `Ddth.Utilities.Tempus`).
- Tests use **MSTest** (`[TestClass]`, `[TestMethod]`, `[TestInitialize]`).
- Test classes are organized into subdirectories by feature area (e.g., `RandomHelper/`, `ReflectionHelper/`), with test data in separate partial class files (`*.Data.cs`).
- Nullable reference types are enabled (`<Nullable>enable</Nullable>`).
- XML doc comments (`<summary>`, `<remarks>`, `<example>`) are used on all public APIs.
- CI tests against .NET 6.x through 10.x. The minimum target framework is `net6.0`.
- Releases use [semantic-release](https://github.com/btnguyen2k/action-semrelease) via GitHub Actions — version bumps are driven by commit messages, not manual edits to `PackageVersion`.
