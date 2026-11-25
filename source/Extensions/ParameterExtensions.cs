using Autodesk.Revit.DB;
using System;
using System.Globalization;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.DB.Parameter"/> and <see cref="Autodesk.Revit.DB.FamilyParameter"/>.</summary>
    public static class ParameterExtensions
    {
        /// <summary>Get the parameter value.</summary>
        /// <remarks>The method internally selects the correct accessor (AsDouble, AsInteger, AsString, or AsElementId) based on the parameter's StorageType.</remarks>
        /// <returns>The parameter value.</returns>
        public static object GetValue(this Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsDouble();

                case StorageType.Integer:
                    return parameter.AsInteger();

                case StorageType.String:
                    return parameter.AsString() ?? string.Empty;

                case StorageType.ElementId:
                    return parameter.AsElementId();

                default:
                    return parameter.AsValueString() ?? string.Empty;
            }
        }

        /// <summary>Sets the parameter to a value.</summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="value">The new value to which the parameter is to be set.</param>
        /// <remarks>Depending on the parameter's storage type, the input value is converted and passed to the corresponding
        /// Set overload — Set(double), Set(int), Set(string), or Set(ElementId).
        /// Text is read as a number with a dot or a comma as the decimal separator, so <c>"2,5"</c> and <c>"2.5"</c> are both 2.5.
        /// An integer parameter takes only whole numbers: <c>2.7</c> is not set.</remarks>
        /// <returns>The SetValue method will return <see langword="true"/> if the parameter was successfully set to the new value, otherwise <see langword="false"/>.</returns>
        public static bool SetValue(this Parameter parameter, object value)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return TryToDouble(value, out var doubleValue) && parameter.Set(doubleValue);

                case StorageType.Integer:
                    if (value is int intValue)
                        return parameter.Set(intValue);

                    return TryToDouble(value, out var number)
                        && number == Math.Truncate(number)
                        && number >= int.MinValue && number <= int.MaxValue
                        && parameter.Set((int)number);

                case StorageType.String:
                    return parameter.Set(value as string ?? value?.ToString());

                case StorageType.ElementId when value is ElementId elementId:
                    return parameter.Set(elementId);

                default:
                    return false;
            }
        }

        // A number from a number, a bool or text. In text a comma is taken as the decimal separator, as a dot is:
        // with the invariant culture alone "2,5" would be read as 25, the comma being its thousands separator.
        private static bool TryToDouble(object value, out double result)
        {
            switch (value)
            {
                case double d:
                    result = d;
                    return true;

                case string text:
                    return double.TryParse(text.Trim().Replace(',', '.'),
                        NumberStyles.Float, CultureInfo.InvariantCulture, out result);

                case IConvertible convertible:
                    try
                    {
                        result = convertible.ToDouble(CultureInfo.InvariantCulture);
                        return true;
                    }
                    catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
                    {
                        result = 0;
                        return false;
                    }

                default:
                    result = 0;
                    return false;
            }
        }

#if R2022_OR_GREATER
        /// <summary>Gets a ForgeTypeId identifying the data type describing values of the parameter.</summary>
        /// <returns>The data type of the parameter, for example <see cref="Autodesk.Revit.DB.SpecTypeId.Length"/>.</returns>
        public static ForgeTypeId GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.GetDataType();
        }

        /// <summary>Gets a ForgeTypeId identifying the data type describing values of the parameter.</summary>
        /// <returns>The data type of the parameter, for example <see cref="Autodesk.Revit.DB.SpecTypeId.Length"/>.</returns>
        public static ForgeTypeId GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.GetDataType();
        }

        /// <summary>Returns the identifier of the parameter definition's parameter group.</summary>
        /// <returns>The group of the parameter, for example <see cref="Autodesk.Revit.DB.GroupTypeId.Data"/>.</returns>
        public static ForgeTypeId GetParameterGroup(this Parameter parameter)
        {
            return parameter.Definition.GetGroupTypeId();
        }

        /// <summary>Returns the identifier of the parameter definition's parameter group.</summary>
        /// <returns>The group of the parameter, for example <see cref="Autodesk.Revit.DB.GroupTypeId.Data"/>.</returns>
        public static ForgeTypeId GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.GetGroupTypeId();
        }
        
#else
        /// <summary>Returns the user visible interpretation of the parameter data.</summary>
        /// <returns>The type of the parameter, for example <see cref="Autodesk.Revit.DB.ParameterType.Length"/>.</returns>
        public static ParameterType GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.ParameterType;
        }

        /// <summary>Returns the user visible interpretation of the parameter data.</summary>
        /// <returns>The type of the parameter, for example <see cref="Autodesk.Revit.DB.ParameterType.Length"/>.</returns>
        public static ParameterType GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterType;
        }

        /// <summary>Returns the group ID of the parameter definition.</summary>
        /// <returns>The group of the parameter, for example <see cref="Autodesk.Revit.DB.BuiltInParameterGroup.PG_DATA"/>.</returns>
        public static BuiltInParameterGroup GetParameterGroup(this Parameter parameter)
        {
            return parameter.Definition.ParameterGroup;
        }

        /// <summary>Returns the group ID of the parameter definition.</summary>
        /// <returns>The group of the parameter, for example <see cref="Autodesk.Revit.DB.BuiltInParameterGroup.PG_DATA"/>.</returns>
        public static BuiltInParameterGroup GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterGroup;
        }
#endif

#if R2021_OR_GREATER
        /// <summary>Gets the identifier of the unit quantifying the parameter value.</summary>
        /// <remarks>The property only applies to parameters of value types.</remarks>
        /// <returns>The unit of the parameter value, for example <see cref="Autodesk.Revit.DB.UnitTypeId.Millimeters"/>.</returns>
        public static ForgeTypeId GetUnitType(this Parameter parameter)
        {
            return parameter.GetUnitTypeId();
        }
#else
        /// <summary>Get the display unit type of the parameter object.</summary>
        /// <returns>The display unit of the parameter value, for example <see cref="Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS"/>.</returns>
        public static DisplayUnitType GetUnitType(this Parameter parameter)
        {
            return parameter.DisplayUnitType;
        }
#endif

        /// <summary>Checks whether a parameter used to control the type of a family nested within another family.</summary>
        /// <returns><see langword="true"/> if the parameter used to control the type of a family nested within another family, <see langword="false"/> otherwise.</returns>
        public static bool IsFamilyType(this FamilyParameter parameter)
        {
#if R2022_OR_GREATER
            return Category.IsBuiltInCategory(parameter.GetParameterType());
#else
            return parameter.GetParameterType() == ParameterType.FamilyType;
#endif
        }
    }
}