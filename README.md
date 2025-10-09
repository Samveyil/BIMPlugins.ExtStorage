# BIMPlugins.ExtStorage

Package with my developments

# Extensions

## ElementIdExtensions

```C#
var wall = new ElementId(123456).ToElement<Wall>();
var parameterFilter = sheetParameter.Id.CreateEqualsFilter("value"); 
```

## ElementExtensions

```C#
var wallType = wall.ToElementType<WallType>();
var bic = wall.GetBuiltInCategory(); 
```

## DocumentExtensions

```C#
var walls = doc.ToElements(BuiltInCategory.OST_Walls);
var floors = doc.ToElements<Floor>(); 
```

## SystemExtensions

```C#
var roundedValue = parameter.AsDouble().Round(5);
var isEmpty = parameter.AsString().IsNullOrEmpty();
var filePath = directoryPath.AppendPath(fileName);
```

# Methods

## ParameterMethods

UnitType no mater of Revit version
"mm",
"cm",
"m",
"m2",
"m3",
"general",
"degrees",
"degreesMinutes",
"W",
"V",

```C#
var areaToInt = UnitUtils.ConvertToInternalUnits(area, ParameterMethods.GetUnitType("m2"));
```

# RevitAPI

Get active document, application, view whenever you want

```C#
var uiapp = RevitAPI.UIApplication;
var app = RevitAPI.Application;
var uidoc = RevitAPI.UIDocument;
var doc = RevitAPI.Document;
var view = RevitAPI.ActiveView;
```