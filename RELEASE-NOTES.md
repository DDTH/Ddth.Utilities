# Ddth.Utilities Release Notes

## 2024-08-26 - v0.1.1

### Fixed/Improvement

- Improvement(ReflectionDIHelper): `BuildDIParams` is now public and has overloads to accept additional services to look up.
- Improvement(ReflectionDIHelper): `CreateInstance<T>` should return `null` if the created instance is not assignable to `T`.

## 2024-08-20 - v0.1.0

### Added/Refactoring

Feature: helper class `ReflectionDIHelper`.
