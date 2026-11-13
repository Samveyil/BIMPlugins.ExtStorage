# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

The package is published separately for each Revit version: version `0.6.0` is released
as `2019.6.0` … `2025.6.0`.

## [0.7.0] / 2026-11-13
### Breaking changes
- `LocationType` moved out of `ElementExtensions` into the `BIMPlugins.ExtStorage.Extensions` namespace:
  write `LocationType.StartPoint` instead of `ElementExtensions.LocationType.StartPoint`.
- `OpenDocument`, `OpenDetachedDocument` and `OpenLocalDocument` no longer show a message box. They still return `null`
  when the file cannot be opened, and new overloads with `out string error` give a message to show to the user.
  The `filePath` parameter is removed from them and from `OpenAndActivateLocalDocument`: the file name is taken
  from the model path. `path.OpenDetachedDocument(filePath)` becomes `path.OpenDetachedDocument()`.
- `NaturalComparer()` now ignores case and compares text by the current culture, as Windows Explorer sorts.
  Use `NaturalComparer.Ordinal` for the old case-sensitive order.
- `ToElementIds<T>` requires `T : Element`, as `ToElements<T>` does.
- `WarningSkipper` deletes the warnings of a transaction even when it also has errors.
### Features
- `NaturalComparer` compares numbers of any length, takes a `StringComparison`, and has ready instances:
  `Ordinal`, `OrdinalIgnoreCase`, `CurrentCulture`, `CurrentCultureIgnoreCase`.
- Double parameter filters (`CreateEqualsFilter`, `CreateNotEqualsFilter`, `CreateGreaterOrEqualFilter`) take a tolerance,
  `1e-6` by default.
- `ExMethods.CreateDirectShape(document, ...)` creates the shape in the given document.
- `SetWarningResolver` is an extension method: `t.SetWarningResolver(new WarningSkipper())`.
### Fixed
- `GetBuiltInCategory` returns `INVALID` for elements without a category instead of throwing,
  so `PickElement(category)` and `PickElements(category)` no longer fail on such elements.
- `ToSolid` returns `null` for elements without geometry instead of throwing.
- `GetView3D` no longer creates a duplicate view (and fails) when the name has characters not allowed in view names,
  and skips view templates.
- `GetSharedParameterGUIDByName` and `GetSharedParameterGUIDsDict` no longer fail on short lines of the shared parameter file.
- `InsertParameter` looks up the shared parameter in the given document, not in the active one.
- Double parameter filters found almost nothing: values were compared with the tolerance `double.Epsilon`.
- `OpeningsCreationSkipper` and `WarningSkipper` check the severity of each failure, not of the whole transaction.
- `BoundingBoxXYZ.Contains(other)` and `Overlaps` are correct for boxes with a rotation.
- `DialogBoxHide` and `WarningDialogHide` also work in the Revit 2025 build.
- `OpenLocalDocument` finds the folder for local files in Revit.ini without a hard-coded user profile path,
  and falls back to the Documents folder.
- `SetOpenOptions` no longer casts the workset list to `List`.
- `SetValue` of a string parameter accepts `null`.
- `PickElements<T>(filter, ...)` skips the elements that are not `T` instead of throwing.
### Updated
- Transaction names are in English.
- Documentation: typos and punctuation.

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

[0.7.0]: https://github.com/Samveyil/BIMPlugins.ExtStorage/compare/v0.6.1...v0.7.0
[0.6.1]: https://github.com/Samveyil/BIMPlugins.ExtStorage/compare/v0.6.0...v0.6.1
[0.6.0]: https://github.com/Samveyil/BIMPlugins.ExtStorage/tree/v0.6.0
