using Autodesk.Revit.DB;
using System;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ParameterExtensions
    {
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
        public static void SetValue(this Parameter parameter, object value)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    parameter.Set(Convert.ToDouble(value));
                    break;

                case StorageType.Integer:
                    parameter.Set(Convert.ToInt32(value));
                    break;

                case StorageType.String:
                    parameter.Set(value.ToString());
                    break;

                case StorageType.ElementId:
                    if (value is ElementId elementId)
                    {
                        parameter.Set(elementId);
                    }
                    break;

                default:
                    throw new InvalidOperationException("Unsupported parameter storage type.");
            }
        }

#if !R2022_OR_GREATER
        public static ParameterType GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.ParameterType;
        }
        public static ParameterType GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterType;
        }
        public static BuiltInParameterGroup GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.ParameterGroup;
        }
        public static DisplayUnitType GetUnitType(this Parameter parameter)
        {
            return parameter.DisplayUnitType;
        }

#else
        public static ForgeTypeId GetParameterType(this Parameter parameter)
        {
            return parameter.Definition.GetDataType();
        }
        public static ForgeTypeId GetParameterType(this FamilyParameter parameter)
        {
            return parameter.Definition.GetDataType();
        }
        public static ForgeTypeId GetParameterGroup(this FamilyParameter parameter)
        {
            return parameter.Definition.GetGroupTypeId();
        }
        public static ForgeTypeId GetUnitType(this Parameter parameter)
        {
            return parameter.GetUnitTypeId();
        }
#endif

        public static bool IsFamilyType(this FamilyParameter parameter)
        {
#if !R2022_OR_GREATER
            return parameter.Definition.ParameterType == ParameterType.FamilyType;
#else
            return Category.IsBuiltInCategory(parameter.Definition.GetDataType());
#endif
        }
    }
}
