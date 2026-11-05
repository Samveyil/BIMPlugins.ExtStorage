# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

The package is published separately for each Revit version: version `0.6.0` is released
as `2019.6.0` … `2025.6.0`.

## [0.6.1] / 2026-11-05
Same library code as `0.6.0`.
### Updated
- Build with `BIMPlugins.Nuke` 1.1.5.

## [0.6.0] / 2026-11-03 - 2026-11-05
First release on nuget.org.
### Features
- Package metadata: MIT license, README, icon, project and repository links.
- Symbol package (`.snupkg`) with portable PDBs.
- Publish to nuget.org from GitHub Actions with Trusted Publishing.
### Updated
- XML documentation for the whole public API.
- `PurgeUnused` finds the purge rule by its id, so it works with any Revit UI language.
- Build Revit 2025 (`net8.0-windows`) with `BIMPlugins.Nuke` 1.1.4.
### Removed
- `SelectionMethods.GetIfcElements` with its filters `IFCParameterFilter` and `IgnoredCategoryFilter`.

[0.6.1]: https://github.com/Samveyil/BIMPlugins.ExtStorage/compare/v0.6.0...v0.6.1
[0.6.0]: https://github.com/Samveyil/BIMPlugins.ExtStorage/tree/v0.6.0
