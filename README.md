# BIMPlugins.ExtStorage

[![Revit 2019-2025](https://img.shields.io/badge/Revit-2019--2025-blue.svg)](https://github.com/Samveyil/BIMPlugins.ExtStorage)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/Samveyil/BIMPlugins.ExtStorage)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Samveyil/BIMPlugins.ExtStorage/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/BIMPlugins.ExtStorage.svg)](https://www.nuget.org/packages/BIMPlugins.ExtStorage)

Extension methods and helpers for the Autodesk Revit API: collecting elements, parameters and units,
user selection, geometry, element transforms, opening documents and failure handling.
One API surface for Revit 2019–2025: version differences are hidden inside the package.

## Installation

The package is built separately for each Revit version. Pick the build that matches the Revit version your add-in targets:

| Revit       | Target framework  | Package version |
|-------------|-------------------|-----------------|
| 2019 – 2024 | `net48`           | `2019.*` … `2024.*` |
| 2025        | `net8.0-windows`  | `2025.*`        |

```xml
<PackageReference Include="BIMPlugins.ExtStorage" Version="$(RevitVersion).*" />
```

The package does not ship `RevitAPI.dll` / `RevitAPIUI.dll`. Reference the Revit API in your project as usual,
for example through `Revit_All_Main_Versions_API_x64`.

```C#
using BIMPlugins.ExtStorage;
using BIMPlugins.ExtStorage.Extensions;
using BIMPlugins.ExtStorage.Extensions.UtilsExtensions;
using BIMPlugins.ExtStorage.Methods;
using BIMPlugins.ExtStorage.FailuresProcessing;
```

## RevitAPI

Static access to the running Revit session, without passing `UIApplication` through every class:

```C#
var uiapp = RevitAPI.UIApplication;
var app   = RevitAPI.Application;
var uidoc = RevitAPI.UIDocument;
var doc   = RevitAPI.Document;
var view  = RevitAPI.ActiveView;
```

Run code from a modeless window (WPF/MVVM) in the Revit API context:

```C#
var updateEvent = RevitAPI.CreateExtEvent(this, vm => vm.UpdateWalls());
updateEvent.Raise();
```

> `RevitAPI` works only inside a running Revit process. `UIDocument`, `Document` and `ActiveView`
> require an open document. Exceptions thrown inside `CreateExtEvent` actions are shown in a `TaskDialog`.

## Collecting elements

```C#
var walls     = doc.ToElements<Wall>();
var floors    = doc.ToElements<Floor>(BuiltInCategory.OST_Floors);
var doors     = doc.ToElements(BuiltInCategory.OST_Doors);
var wallTypes = doc.ToElements(BuiltInCategory.OST_Walls, toTypes: true);
var onView    = doc.ToElements<FamilyInstance>(view.Id, BuiltInCategory.OST_Furniture);

var wallIds   = doc.ToElementIds<Wall>();
var model     = doc.ToModelElements();      // model elements of all model categories
```

Every overload accepts an optional `ElementFilter` and/or a view id.

## Parameter filters

Filters are created from the parameter id:

```C#
var markId = new ElementId(BuiltInParameter.ALL_MODEL_MARK);

var walls = doc.ToElements<Wall>(markId.CreateBeginsWithFilter("W-"));
var empty = doc.ToElements<Wall>(markId.CreateHasNoValueFilter());
```

Available: `CreateEqualsFilter`, `CreateNotEqualsFilter`, `CreateGreaterOrEqualFilter`,
`CreateContainsFilter`, `CreateNotContainsFilter`, `CreateBeginsWithFilter`, `CreateNotBeginsWithFilter`,
`CreateEndsWithFilter`, `CreateNotEndsWithFilter`, `CreateHasValueFilter`, `CreateHasNoValueFilter`.

## Elements and ids

```C#
var wall     = id.ToElement<Wall>();            // uses the active document when doc is not passed
var wallType = wall.ToElementType<WallType>();
var bic      = wall.GetBuiltInCategory();
var param    = wall.ToParameter("Comments");
var start    = wall.ToPoint(ElementExtensions.LocationType.StartPoint);
var line     = wall.ToLine();
var solid    = wall.ToSolid();
var value    = id.GetValue();                   // int before Revit 2024, long since 2024
```

## Parameters

```C#
object value = param.GetValue();   // AsDouble / AsInteger / AsString / AsElementId by StorageType
param.SetValue(1500.0.FromMillimeters());
param.SetValue("Text");
```

Shared parameters:

```C#
var guid       = ParameterMethods.GetSharedParameterGUIDByName("BP_Section");
var definition = ParameterMethods.FindExternalDefinition("BP_Section", guid);

doc.ParameterBindings.InsertParameter(doc, "BP_Section", guid, categories, isInstance: true);
```

## Units

```C#
double internalLength = 1500.0.FromMillimeters();
double millimeters    = wall.Width.ToMillimeters();
double radians        = 45.0.FromDegrees();
double area           = room.Area.ToUnit("m2");
```

Supported unit strings: `mm`, `cm`, `m`, `m2`, `m3`, `general`, `degrees`, `degreesMinutes`, `W`, `V`.
An unknown string falls back to millimeters.

## Selection

```C#
var wall   = uidoc.PickElement<Wall>("Select a wall");
var doors  = uidoc.PickElements(BuiltInCategory.OST_Doors, "Select doors");
var point  = uidoc.PickPoint("Pick a point");
var picked = uidoc.ToSelectedElements();
```

If the user presses Esc, `Pick*` methods return `null` instead of throwing.

## Transforms and draw order

Call these inside a transaction:

```C#
wall.Move(new XYZ(10, 0, 0));
wall.Rotate(axis, Math.PI / 2);
var copies = ids.CopyElements(sourceDoc, targetDoc);

detailLine.BringToFront(view);
```

## Geometry

```C#
var flat = point.SetZ(0);
bool inside   = bbox.Contains(point);
bool overlaps = bbox.Overlaps(otherBox);
var parts     = solid.SplitVolumes();
```

## Documents

```C#
doc.SynchronizeWithCentral("Daily sync");          // relinquishes all worksets and elements
var view3D = doc.GetView3D("Navisworks");          // finds or creates the view and activates it

var path   = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath);
var detached = path.OpenDetachedDocument(filePath);
var local    = path.OpenLocalDocument(filePath);   // replaces an existing local copy

doc.PurgeUnused();                                 // like Purge Unused, including materials; opens its own transactions
```

> `OpenDocument` and its variants show an error dialog and return `null` when the file cannot be opened.

## Failure handling

Hide warnings during a transaction:

```C#
using var t = new Transaction(doc, "Create walls");
t.Start();
TransactionHandler.SetWarningResolver(t, new WarningSkipper());
// ...
t.Commit();
```

Also available: `OpeningsCreationSkipper`, `FamilyLoadOptions`, `DuplicateTypeNamesHandler`.

```C#
doc.LoadFamily(familyPath, new FamilyLoadOptions(overwriteParameters: true), out var family);
```

## Other helpers

```C#
var sorted = names.OrderBy(n => n, new NaturalComparer());   // "Level 2" before "Level 10"
var rounded = value.Round(5);
var file    = directory.AppendPath("model.rvt");
var rel     = rootFolder.GetRelativePath(filePath);
```

## Differences between Revit versions

The method names stay the same, but some return types follow the Revit API of each version:

| Method | Before | Since |
|--------|--------|-------|
| `ElementId.GetValue()` | `int` | `long` (2024) |
| `ParameterMethods.GetUnitType()`, `Parameter.GetUnitType()`, `FromUnit`/`ToUnit` | `DisplayUnitType` | `ForgeTypeId` (2021) |
| `GetParameterType()`, `GetParameterGroup()` | `ParameterType`, `BuiltInParameterGroup` | `ForgeTypeId` (2022) |
| `BindingMap.InsertParameter(..., group)` | `BuiltInParameterGroup` | `ForgeTypeId` (2022) |

## License

This project is [licensed](https://github.com/Samveyil/BIMPlugins.ExtStorage/blob/master/LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/Samveyil/BIMPlugins.ExtStorage/stargazers)!
