using Autodesk.Revit.DB;
using System;

namespace BIMPlugins.ExtStorage.Extensions
{
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
        /// <param name="value">The new value to which the parameter is to be set.</param>
        /// <remarks>Depending on the parameter's storage type, the input value is converted and passed to the corresponding
        /// Set overload — Set(double), Set(int), Set(string), or Set(ElementId).</remarks>
        /// <returns>The SetValue method will return True if the parameter was successfully set to the new value, otherwise false.</returns>
        public static bool SetValue(this Parameter parameter, object value)
        {
            return parameter.StorageType switch
            {
                StorageType.Double => parameter.Set(Convert.ToDouble(value)),
                StorageType.Integer => parameter.Set(Convert.ToInt32(value)),
                StorageType.String => parameter.Set(value.ToString()),
                StorageType.ElementId when value is ElementId elementId => parameter.Set(elementId),
                _ => false
            };
        }

#if !R2022_OR_GREATER
        /// <summary>Returns the user visible interpretation of the parameter data.</summary>
        public static ParameterType GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.ParameterType;
        }

        /// <summary>Returns the user visible interpretation of the parameter data.</summary>
        public static ParameterType GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterType;
        }

        /// <summary>Returns the group ID of the parameter definition.</summary>
        public static BuiltInParameterGroup GetParameterGroup(this Parameter parameter)
        {
            return parameter.Definition.ParameterGroup;
        }

        /// <summary>Returns the group ID of the parameter definition.</summary>
        public static BuiltInParameterGroup GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterGroup;
        }

        /// <summary>Get the display unit type of the parameter object.</summary>
        public static DisplayUnitType GetUnitType(this Parameter parameter)
        {
            return parameter.DisplayUnitType;
        }

#else
        /// <summary>Gets a ForgeTypeId identifying the data type describing values of the parameter.</summary>
        public static ForgeTypeId GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.GetDataType();
        }

        /// <summary>Gets a ForgeTypeId identifying the data type describing values of the parameter.</summary>
        public static ForgeTypeId GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.GetDataType();
        }

        /// <summary>Returns the identifier of the parameter definition's parameter group.</summary>
        public static ForgeTypeId GetParameterGroup(this Parameter parameter)
        {
            return parameter.Definition.GetGroupTypeId();
        }

        /// <summary>Returns the identifier of the parameter definition's parameter group.</summary>
        public static ForgeTypeId GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.GetGroupTypeId();
        }

        /// <summary>Gets the identifier of the unit quantifying the parameter value.</summary>
        /// <remarks>The property only applies to parameters of value types.</remarks>
        public static ForgeTypeId GetUnitType(this Parameter parameter)
        {
            return parameter.GetUnitTypeId();
        }
#endif

        /// <summary>Checks whether a parameter used to control the type of a family nested within another family.</summary>
        /// <returns>True if the parameter used to control the type of a family nested within another family, false otherwise.</returns>
        public static bool IsFamilyType(this FamilyParameter parameter)
        {
#if !R2022_OR_GREATER
            return parameter.GetParameterType() == ParameterType.FamilyType;
#else
            return Category.IsBuiltInCategory(parameter.GetParameterType());
#endif
        }
    }
}