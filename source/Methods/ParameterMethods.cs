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

        public static SharedParameterElement GetSharedParameterByName(string name)
        {
            return new FilteredElementCollector(RevitAPI.Document)
                .OfClass(typeof(SharedParameterElement))
                .FirstOrDefault(p => p.Name == name) as SharedParameterElement;
        }

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
                MessageBox.Show("Укажите файл ФОП в Revit", "Ошибка в работе плагина", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return guids;
        }

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
                MessageBox.Show("Укажите файл ФОП в Revit", "Ошибка в работе плагина", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Guid();
            }

            return guid;
        }

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
                MessageBox.Show("Укажите файл ФОП в Revit", "Ошибка в работе плагина", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return guidsDict;
        }

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