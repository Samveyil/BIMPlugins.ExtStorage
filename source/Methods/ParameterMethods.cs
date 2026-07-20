using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace BIMPlugins.ExtStorage.Methods
{
    public static class ParameterMethods
    {
#if !R2021_OR_GREATER
        /// <summary>Converts a unit type string to its corresponding <see cref="DisplayUnitType"/> enumeration value.</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>
        /// The <see cref="DisplayUnitType"/> corresponding to the input string.
        /// If the input is not recognized, defaults to <see cref="DisplayUnitType.DUT_MILLIMETERS"/>.
        /// </returns>
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
                _ => DisplayUnitType.DUT_MILLIMETERS
            };
        }

#else
        /// <summary>Converts a unit type string to its corresponding <see cref="ForgeTypeId"/> unit type identifier.</summary>
        /// <param name="unitType">The unit type string. Supported values: <c>mm</c>, <c>cm</c>, <c>m</c>, <c>m2</c>, <c>m3</c>, <c>general</c>, <c>degrees</c>, <c>degreesMinutes</c>, <c>W</c>, <c>V</c>.</param>
        /// <returns>
        /// The <see cref="ForgeTypeId"/> corresponding to the input string.
        /// If the input is not recognized, defaults to <see cref="UnitTypeId.Millimeters"/>.
        /// </returns>
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
                _ => UnitTypeId.Millimeters
            };
        }
#endif

        /// <summary>Retrieves a list of GUIDs of all shared parameters defined in the current shared parameter file.</summary>
        /// <returns>A list of <see cref="Guid"/> objects representing the shared parameter GUIDs, or <see langword="null"/> if the shared parameter file is not specified.</returns>
        public static List<Guid> GetSharedParameterGUIDs()
        {
            List<Guid> guids = new List<Guid>();

            try
            {
                using (StreamReader sr = new StreamReader(RevitAPI.Application.SharedParametersFilename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("PARAM"))
                        {
                            string[] parts = line.Split('\t');
                            Guid paramGuid;
                            if (parts.Length > 1 && Guid.TryParse(parts[1], out paramGuid))
                            {
                                guids.Add(paramGuid);
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return null;
            }

            return guids;
        }

        /// <summary>Gets the GUID of a shared parameter by its name from the current shared parameter file.</summary>
        /// <param name="parameterName">The name of the shared parameter to search for.</param>
        /// <returns>The <see cref="Guid"/> of the shared parameter if found; otherwise, an empty <see cref="Guid"/>.</returns>
        public static Guid GetSharedParameterGUIDByName(string parameterName)
        {
            Guid guid = new Guid();

            try
            {
                using (StreamReader sr = new StreamReader(RevitAPI.Application.SharedParametersFilename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("PARAM"))
                        {
                            string[] parts = line.Split('\t');
                            if (parts.Length > 1 && parts[2] == parameterName && Guid.TryParse(parts[1], out guid))
                            {
                                return guid;
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return new Guid();
            }

            return guid;
        }

        /// <summary>Gets a dictionary of all shared parameter names and their corresponding GUIDs from the current shared parameter file.</summary>
        /// <returns>
        /// A <see cref="Dictionary{string, Guid}"/> where the key is the parameter name and the value is the parameter GUID.
        /// Returns <see langword="null"/> if the shared parameter file is not configured.
        /// </returns>
        public static Dictionary<string, Guid> GetSharedParameterGUIDsDict()
        {
            Dictionary<string, Guid> guidsDict = [];

            try
            {
                using (StreamReader sr = new StreamReader(RevitAPI.Application.SharedParametersFilename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("PARAM"))
                        {
                            string[] parts = line.Split('\t');
                            Guid paramGuid;
                            if (parts.Length > 1 && Guid.TryParse(parts[1], out paramGuid))
                            {
                                guidsDict[parts[2]] = paramGuid;
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return null;
            }

            return guidsDict;
        }

        /// <summary>Retrieves an <see cref="ExternalDefinition"/> by parameter name and GUID from the current shared parameter file.</summary>
        /// <param name="parameterName">The name of the shared parameter to search for.</param>
        /// <param name="guid">The GUID of the shared parameter to search for.</param>
        /// <returns>The <see cref="ExternalDefinition"/> if found; otherwise, <see langword="null"/>.</returns>
        /// <remarks>
        /// Both the <paramref name="parameterName"/> and <paramref name="guid"/> must match for the definition to be returned.
        /// Returns <see langword="null"/> if the shared parameter file is not configured or no matching definition is found.
        /// </remarks>
        public static ExternalDefinition FindExternalDefinition(string parameterName, Guid guid)
        {
            foreach (DefinitionGroup group in RevitAPI.Application.OpenSharedParameterFile().Groups)
            {
                foreach (ExternalDefinition definition in group.Definitions)
                {
                    if (definition.Name == parameterName && definition.GUID == guid)
                    {
                        return definition;
                    }
                }
            }
            return null;
        }
    }
}