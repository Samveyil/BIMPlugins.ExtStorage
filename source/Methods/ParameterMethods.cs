using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BIMPlugins.ExtStorage.Methods
{
    /// <summary>Helpers for unit types and shared parameters.</summary>
    public static class ParameterMethods
    {
        private const string SupportedUnitTypes = "mm, cm, m, m2, m3, general, degrees, degreesMinutes, W, V";

#if !R2021_OR_GREATER
        /// <summary>Converts a unit type string to its corresponding <see cref="Autodesk.Revit.DB.DisplayUnitType"/> enumeration value.</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>The <see cref="Autodesk.Revit.DB.DisplayUnitType"/> corresponding to the input string.</returns>
        /// <exception cref="T:System.ArgumentException">The unit type is not one of the supported values.</exception>
        public static DisplayUnitType GetUnitType(string unitType = "mm")
        {
            return unitType switch
            {
                "mm" => DisplayUnitType.DUT_MILLIMETERS,
                "cm" => DisplayUnitType.DUT_CENTIMETERS,
                "m" => DisplayUnitType.DUT_METERS,
                "m2" => DisplayUnitType.DUT_SQUARE_METERS,
                "m3" => DisplayUnitType.DUT_CUBIC_METERS,
                "general" => DisplayUnitType.DUT_GENERAL,
                "degrees" => DisplayUnitType.DUT_DECIMAL_DEGREES,
                "degreesMinutes" => DisplayUnitType.DUT_DEGREES_AND_MINUTES,
                "W" => DisplayUnitType.DUT_WATTS,
                "V" => DisplayUnitType.DUT_VOLTS,
                _ => throw UnknownUnitType(unitType)
            };
        }

#else
        /// <summary>Converts a unit type string to its corresponding <see cref="Autodesk.Revit.DB.ForgeTypeId"/> unit type identifier.</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>The <see cref="Autodesk.Revit.DB.ForgeTypeId"/> corresponding to the input string.</returns>
        /// <exception cref="T:System.ArgumentException">The unit type is not one of the supported values.</exception>
        public static ForgeTypeId GetUnitType(string unitType = "mm")
        {
            return unitType switch
            {
                "mm" => UnitTypeId.Millimeters,
                "cm" => UnitTypeId.Centimeters,
                "m" => UnitTypeId.Meters,
                "m2" => UnitTypeId.SquareMeters,
                "m3" => UnitTypeId.CubicMeters,
                "general" => UnitTypeId.General,
                "degrees" => UnitTypeId.Degrees,
                "degreesMinutes" => UnitTypeId.DegreesMinutes,
                "W" => UnitTypeId.Watts,
                "V" => UnitTypeId.Volts,
                _ => throw UnknownUnitType(unitType)
            };
        }
#endif

        // A typo such as "mm2" must not be converted as millimetres without a word.
        private static ArgumentException UnknownUnitType(string unitType) =>
            new ArgumentException($"Unknown unit type \"{unitType}\". Supported: {SupportedUnitTypes}.", nameof(unitType));

        /// <summary>Retrieves a list of GUIDs of all shared parameters defined in the current shared parameter file.</summary>
        /// <returns>
        /// A list of <see cref="Guid"/> objects representing the shared parameter GUIDs,
        /// or <see langword="null"/> if the shared parameter file is not specified, does not exist or cannot be read.
        /// </returns>
        public static List<Guid> GetSharedParameterGUIDs()
        {
            return ReadSharedParameters()?
                .Select(p => p.Guid)
                .ToList();
        }

        /// <summary>Gets the GUID of a shared parameter by its name from the current shared parameter file.</summary>
        /// <param name="parameterName">The name of the shared parameter to search for.</param>
        /// <returns>
        /// The <see cref="Guid"/> of the shared parameter if found; otherwise <see cref="Guid.Empty"/>, also when the
        /// shared parameter file is not specified, does not exist or cannot be read.
        /// </returns>
        public static Guid GetSharedParameterGUIDByName(string parameterName)
        {
            return ReadSharedParameters()?
                .FirstOrDefault(p => p.Name == parameterName)
                .Guid ?? Guid.Empty;
        }

        /// <summary>Gets a dictionary of all shared parameter names and their corresponding GUIDs from the current shared parameter file.</summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> where the key is the parameter name and the value is the parameter GUID.
        /// Returns <see langword="null"/> if the shared parameter file is not specified, does not exist or cannot be read.
        /// </returns>
        public static Dictionary<string, Guid> GetSharedParameterGUIDsDict()
        {
            var parameters = ReadSharedParameters();
            if (parameters == null)
                return null;

            var guidsDict = new Dictionary<string, Guid>();
            foreach (var (guid, name) in parameters)
                guidsDict[name] = guid;

            return guidsDict;
        }

        // The GUID and the name of each PARAM line of the shared parameter file; null when the file is not specified,
        // does not exist or cannot be read. File.ReadLines detects the UTF-16 byte order mark of the file.
        private static List<(Guid Guid, string Name)> ReadSharedParameters()
        {
            var path = RevitAPI.Application.SharedParametersFilename;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;

            try
            {
                var parameters = new List<(Guid Guid, string Name)>();
                foreach (var line in File.ReadLines(path))
                {
                    if (!line.StartsWith("PARAM"))
                        continue;

                    var parts = line.Split('\t');
                    if (parts.Length > 2 && Guid.TryParse(parts[1], out var guid))
                        parameters.Add((guid, parts[2]));
                }

                return parameters;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return null;
            }
        }

        /// <summary>Retrieves an <see cref="Autodesk.Revit.DB.ExternalDefinition"/> by parameter name and GUID from the current shared parameter file.</summary>
        /// <param name="parameterName">The name of the shared parameter to search for.</param>
        /// <param name="guid">The GUID of the shared parameter to search for.</param>
        /// <returns>The <see cref="Autodesk.Revit.DB.ExternalDefinition"/> if found; otherwise, <see langword="null"/>.</returns>
        /// <remarks>
        /// Both the <paramref name="parameterName"/> and <paramref name="guid"/> must match for the definition to be returned.
        /// Returns <see langword="null"/> if the shared parameter file is not configured or no matching definition is found.
        /// </remarks>
        public static ExternalDefinition FindExternalDefinition(string parameterName, Guid guid)
        {
            var file = RevitAPI.Application.OpenSharedParameterFile();
            if (file == null)
                return null;

            foreach (DefinitionGroup group in file.Groups)
            {
                foreach (ExternalDefinition definition in group.Definitions)
                {
                    if (definition.Name == parameterName && definition.GUID == guid)
                        return definition;
                }
            }

            return null;
        }
    }
}
