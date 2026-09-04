using Autodesk.Revit.DB;
using BIMPlugins.ExtStorage.Methods;

namespace BIMPlugins.ExtStorage.Extensions.UtilsExtensions
{
    public static class UnitUtilsExtensions
    {
        /// <summary>Converts the specified unit to internal Revit format</summary>
        /// <returns>The converted value</returns>
#if R2021_OR_GREATER
        public static double FromUnit(this double value, ForgeTypeId unitId)
        {
            return UnitUtils.ConvertToInternalUnits(value, unitId);
        }
#else
        public static double FromUnit(this double value, DisplayUnitType unitId)
        {
            return UnitUtils.ConvertToInternalUnits(value, unitId);
        }
#endif

        /// <summary>Converts the specified unit to internal Revit format</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>The converted value</returns>
        public static double FromUnit(this double value, string unitType)
        {
            return UnitUtils.ConvertToInternalUnits(value, ParameterMethods.GetUnitType(unitType));
        }

        /// <summary>Converts a Revit internal format value to the specified unit</summary>
        /// <returns>The converted value</returns>
#if R2021_OR_GREATER
        public static double ToUnit(this double value, ForgeTypeId unitId)
        {
            return UnitUtils.ConvertFromInternalUnits(value, unitId);
        }
#else
        public static double ToUnit(this double value, DisplayUnitType unitId)
        {
            return UnitUtils.ConvertFromInternalUnits(value, unitId);
        }
#endif

        /// <summary>Converts a Revit internal format value to the specified unit</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>The converted value</returns>
        public static double ToUnit(this double value, string unitType)
        {
            return UnitUtils.ConvertFromInternalUnits(value, ParameterMethods.GetUnitType(unitType));
        }

        /// <summary>Converts millimeters to internal Revit format</summary>
        /// <returns>Value in feet</returns>
        public static double FromMillimeters(this double value) => value.FromUnit("mm");

        /// <summary>Converts a Revit internal format value to millimeters</summary>
        /// <returns>Value in millimeters</returns>
        public static double ToMillimeters(this double value) => value.ToUnit("mm");

        /// <summary>Converts degrees to internal Revit format</summary>
        /// <returns>Value in radians</returns>
        public static double FromDegrees(this double value) => value.FromUnit("degrees");

        /// <summary>Converts a Revit internal format value to degrees</summary>
        /// <returns>Value in degrees</returns>
        public static double ToDegrees(this double value) => value.ToUnit("degrees");
    }
}